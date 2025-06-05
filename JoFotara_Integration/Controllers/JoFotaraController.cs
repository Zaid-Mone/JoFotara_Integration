using JoFotara_Integration.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace JoFotara_Integration.Controllers;

[Route("api/[controller]/[Action]")]
[ApiController]
public class JoFotaraController : ControllerBase
{

    /// <summary>
    /// ملاحظة :
    /// تم ارفاق  ملف XML
    /// موجود بداخل مجلد اسمه 
    ///wwwroot/XML/SendingFawtaraXML.xml
    /// </summary>
    private readonly IConfiguration _configuration;
    private readonly JoFotaraCredentials _joFotara;
    public JoFotaraController(IConfiguration configuration)
    {
        _configuration = configuration;
        _joFotara = configuration.GetSection("JoFotara").Get<JoFotaraCredentials>();
    }








    /// <summary>
    /// This is API endpoint to send an invoice to JoFotara.
    /// In this Approach, we build the XML structure manually and send it to JoFotara API.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> SendFotaraInvoiceV1()
    {
        try
        {
            JoFotaraRequest fotara = new JoFotaraRequest()
            {
                IENV_Fotara = new IENV_Fotara()
                {
                    ID = $"{7295762}",
                    IENV_DATE = DateTime.Now.ToString("yyyy-MM-dd"),
                    IENV_UUID = $"{Guid.NewGuid()}",
                    IENV_TABLE_SERIAL_ID = $"{1001}",
                    IENV_Activity_Number = $"{_joFotara.Activity_Number}",
                    IENV_COMPANY_COMMERCIAL_NUMBER = $"{_joFotara.Company_Commercial_Number}",
                    IENV_COMPANY_NAME = _joFotara.Company_Name,
                    IENV_COMMERCIAL_NUMBER = "your customer commercial number",
                    IENV_CUSTOMER_COMMERCIAL_NUMBER = "Customer Commercial number",
                    IENV_CUSTOMER_NAME = "Customer Name",
                    IENV_CUSTOMER_PHONE = "Customer Phone Number",
                    IENV_NOTE = "Test Note",
                    IENV_POSTAL_CODE = "33554",
                    IENV_FAWTARA_CITY_NAME = "JO-AZ",
                    IENV_TOTAL_AMT_WITHOUT_DISCOUNT = "1.00",
                    IENV_TOTAL_AMT_WITH_DISCOUNT = "1.10",
                    IENV_TOTAL_INVOICE = "1.10",
                },
                InvoiceLineDetails = this.GetDummayListInvoices().Select((n, index) => new IENV_InvoiceLineDetails
                {
                    IENV_ID = $"{index + 1}",
                    IENV_PCE = $"{n.Quantity}",
                    INEV_ITEM_NAME = n.ProductName,
                    IENV_PRICE_AMOUNT = (n.Price).ToString(),
                    IENV_ROUNDING_AMOUNT = (n.Price * n.Quantity).ToString(),
                }).ToList()
            };

            #region XML_GENERATION
            XNamespace defaultNs = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2";
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            XNamespace ext = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2";

            var xmlDoc = new XDocument(
                new XElement(defaultNs + "Invoice",
                    new XAttribute(XNamespace.Xmlns + "cac", cac),
                    new XAttribute(XNamespace.Xmlns + "cbc", cbc),
                    new XAttribute(XNamespace.Xmlns + "ext", ext),
                    new XElement(cbc + "ProfileID", "reporting:1.0"),
                    new XElement(cbc + "ID", fotara.IENV_Fotara.IENV_TABLE_SERIAL_ID),
                    new XElement(cbc + "UUID", fotara.IENV_Fotara.IENV_UUID),
                    new XElement(cbc + "IssueDate", fotara.IENV_Fotara.IENV_DATE),
                    new XElement(cbc + "InvoiceTypeCode", new XAttribute("name", "011"), "388"), // 388 this is sending , 011 it mean it  is cash
                    new XElement(cbc + "Note", fotara.IENV_Fotara.IENV_NOTE),
                    new XElement(cbc + "DocumentCurrencyCode", "JOD"),
                    new XElement(cbc + "TaxCurrencyCode", "JOD"),

                    new XElement(cac + "AdditionalDocumentReference",
                        new XElement(cbc + "ID", "ICV"),
                        new XElement(cbc + "UUID", fotara.IENV_Fotara.ID)
                    ),

                    new XElement(cac + "AccountingSupplierParty",
                        new XElement(cac + "Party",
                            new XElement(cac + "PostalAddress",
                                new XElement(cac + "Country",
                                    new XElement(cbc + "IdentificationCode", "JO")
                                )
                            ),
                            new XElement(cac + "PartyTaxScheme",
                                new XElement(cbc + "CompanyID", fotara.IENV_Fotara.IENV_COMPANY_COMMERCIAL_NUMBER),
                                new XElement(cac + "TaxScheme",
                                    new XElement(cbc + "ID", "VAT")
                                )
                            ),
                            new XElement(cac + "PartyLegalEntity",
                                new XElement(cbc + "RegistrationName", fotara.IENV_Fotara.IENV_COMPANY_NAME)
                            )
                        )
                    ),

                    new XElement(cac + "AccountingCustomerParty",
                        new XElement(cac + "Party",
                            new XElement(cac + "PartyIdentification",
                                new XElement(cbc + "ID",
                                    new XAttribute("schemeID", "TN"),
                                    fotara.IENV_Fotara.IENV_COMMERCIAL_NUMBER)
                            ),
                            new XElement(cac + "PostalAddress",
                                new XElement(cbc + "PostalZone", fotara.IENV_Fotara.IENV_POSTAL_CODE),
                                new XElement(cbc + "CountrySubentityCode", fotara.IENV_Fotara.IENV_FAWTARA_CITY_NAME),
                                new XElement(cac + "Country",
                                    new XElement(cbc + "IdentificationCode", "JO")
                                )
                            ),
                            new XElement(cac + "PartyTaxScheme",
                                new XElement(cbc + "CompanyID", fotara.IENV_Fotara.IENV_COMMERCIAL_NUMBER),
                                new XElement(cac + "TaxScheme",
                                    new XElement(cbc + "ID", "VAT")
                                )
                            ),
                            new XElement(cac + "PartyLegalEntity",
                                new XElement(cbc + "RegistrationName", fotara.IENV_Fotara.IENV_CUSTOMER_NAME)
                            )
                        ),
                        new XElement(cac + "AccountingContact",
                            new XElement(cbc + "Telephone", fotara.IENV_Fotara.IENV_CUSTOMER_PHONE)
                        )
                    ),

                    new XElement(cac + "SellerSupplierParty",
                        new XElement(cac + "Party",
                            new XElement(cac + "PartyIdentification",
                                new XElement(cbc + "ID", fotara.IENV_Fotara.IENV_Activity_Number)
                            )
                        )
                    ),

                    new XElement(cac + "AllowanceCharge",
                        new XElement(cbc + "ChargeIndicator", "false"),
                        new XElement(cbc + "AllowanceChargeReason", "discount"),
                        new XElement(cbc + "Amount", new XAttribute("currencyID", "JO"), "0.00")
                    ),

                    new XElement(cac + "TaxTotal",
                        new XElement(cbc + "TaxAmount", new XAttribute("currencyID", "JO"), "0.00")
                    ),

                    new XElement(cac + "LegalMonetaryTotal",
                        new XElement(cbc + "TaxExclusiveAmount",
                            new XAttribute("currencyID", "JO"),
                            fotara.IENV_Fotara.IENV_TOTAL_AMT_WITHOUT_DISCOUNT),
                        new XElement(cbc + "TaxInclusiveAmount",
                            new XAttribute("currencyID", "JO"),
                            fotara.IENV_Fotara.IENV_TOTAL_AMT_WITH_DISCOUNT),
                        new XElement(cbc + "AllowanceTotalAmount",
                            new XAttribute("currencyID", "JO"), "0"),
                        new XElement(cbc + "PayableAmount",
                            new XAttribute("currencyID", "JO"),
                            fotara.IENV_Fotara.IENV_TOTAL_INVOICE)
                    ),

                    fotara.InvoiceLineDetails?.Select((detail, index) =>
                        new XElement(cac + "InvoiceLine",
                            new XElement(cbc + "ID", detail.IENV_ID),
                            new XElement(cbc + "InvoicedQuantity",
                                new XAttribute("unitCode", "PCE"),
                                detail.IENV_PCE),
                            new XElement(cbc + "LineExtensionAmount",
                                new XAttribute("currencyID", "JO"),
                                detail.IENV_TOTAL_LINE_EXTENSION ?? detail.IENV_ROUNDING_AMOUNT),
                            new XElement(cac + "TaxTotal",
                                new XElement(cbc + "TaxAmount",
                                    new XAttribute("currencyID", "JO"),
                                    "0.00"),
                                new XElement(cbc + "RoundingAmount",
                                    new XAttribute("currencyID", "JO"),
                                    detail.IENV_ROUNDING_AMOUNT),
                                new XElement(cac + "TaxSubtotal",
                                    new XElement(cbc + "TaxAmount",
                                        new XAttribute("currencyID", "JO"),
                                        "0.00"),
                                    new XElement(cac + "TaxCategory",
                                        new XElement(cbc + "ID",
                                            new XAttribute("schemeAgencyID", "6"),
                                            new XAttribute("schemeID", "UN/ECE 5305"),
                                            "Z"),
                                        new XElement(cbc + "Percent", "0.00"),
                                        new XElement(cac + "TaxScheme",
                                            new XElement(cbc + "ID",
                                                new XAttribute("schemeAgencyID", "6"),
                                                new XAttribute("schemeID", "UN/ECE 5153"),
                                                "VAT"))))),
                            new XElement(cac + "Item",
                                new XElement(cbc + "Name", detail.INEV_ITEM_NAME)),
                            new XElement(cac + "Price",
                                new XElement(cbc + "PriceAmount",
                                    new XAttribute("currencyID", "JO"),
                                    detail.IENV_PRICE_AMOUNT),
                                new XElement(cac + "AllowanceCharge",
                                    new XElement(cbc + "ChargeIndicator", "false"),
                                    new XElement(cbc + "AllowanceChargeReason", "DISCOUNT"),
                                    new XElement(cbc + "Amount",
                                        new XAttribute("currencyID", "JO"),
                                        "0.00"))))
                    ) ?? new XElement[0]
                )
            );
            #endregion






            #region Sending Fotara (pick 1 or 2)
            #region 1
            string finalXmlContent = xmlDoc.ToString();
            string _Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(finalXmlContent));

            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, _joFotara.ApiUrl);
            request.Headers.Add("Client-Id", _joFotara.Client_Id);
            request.Headers.Add("Secret-Key", _joFotara.Secret_Key);
            request.Headers.Add("Cookie", "stickounet=20f63c41d2e23dc62a6e581cd42905c8|7480c8b0e4ce7933ee164081a50488f1");

            var jsonContent = new
            {
                invoice = _Encoded
            };

            var content = new StringContent(JsonSerializer.Serialize(jsonContent), Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await client.SendAsync(request);
            var _Content = await response.Content.ReadAsStringAsync();
            var _ResponseContent = JsonSerializer.Deserialize<EINV_Response>(_Content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(_ResponseContent?.EINV_RESULTS?.status ?? "Success");
            }
            else
            {
                return BadRequest($"ERROR: {_ResponseContent?.EINV_RESULTS?.ERRORS}");
            }
            #endregion

            #region 2


            (string content, bool IsSuccess) _response = await SendFotaraRequest(finalXmlContent);

            var _ResponseContent1 = JsonSerializer.Deserialize<EINV_Response>(_response.content);

            if (_response.IsSuccess)
            {
                return Ok(_ResponseContent?.EINV_RESULTS?.status ?? "Success");
            }
            else
            {
                return BadRequest($"ERROR: {_ResponseContent?.EINV_RESULTS?.ERRORS}");
            }
            #endregion
            #endregion
        }
        catch (Exception ex)
        {
            return BadRequest($"ERROR: {ex.Message}");
        }
    }


    /// <summary>
    /// Send Request to Fotara
    /// </summary>
    private async Task<(string content, bool IsSuccess)> SendFotaraRequest(string xmlContent)
    {
        string _Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlContent));

        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, _joFotara.ApiUrl);
        request.Headers.Add("Client-Id", _joFotara.Client_Id);
        request.Headers.Add("Secret-Key", _joFotara.Secret_Key);
        request.Headers.Add("Cookie", "stickounet=20f63c41d2e23dc62a6e581cd42905c8|7480c8b0e4ce7933ee164081a50488f1");

        var jsonContent = new
        {
            invoice = _Encoded
        };

        var content = new StringContent(JsonSerializer.Serialize(jsonContent), Encoding.UTF8, "application/json");
        request.Content = content;

        var response = await client.SendAsync(request);
        var _Content = await response.Content.ReadAsStringAsync();
        return (_Content, response.IsSuccessStatusCode);

    }



    /// <summary>
    /// For demo purposes, this method returns a list of dummy invoices.
    /// </summary>
    private List<DummayListInvoice> GetDummayListInvoices()
    {
        return new List<DummayListInvoice>()
        {
            new DummayListInvoice() { Quantity = 1, ProductName = "Product 1", Price = 100 },
            new DummayListInvoice() { Quantity = 2, ProductName = "Product 2", Price = 200 },
            new DummayListInvoice() { Quantity = 3, ProductName = "Product 3", Price = 300 }
        };
    }
}
