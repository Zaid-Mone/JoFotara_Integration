namespace JoFotara_Integration.DTOs;

public class JoFotaraRequest
{
    public IENV_Fotara IENV_Fotara { get; set; }
    public List<IENV_InvoiceLineDetails> InvoiceLineDetails { get; set; }
}




/// <summary>
/// This class represents the main invoice data structure for JoFotara integration.
/// </summary>
public class IENV_Fotara
{
    public string IENV_TABLE_SERIAL_ID { get; set; } // you can use this as the Invoice ID, it should be unique for each invoice (required)
    public string ID { get; set; } // This is the invoice ID, you can use  you table serial ID as the invoice ID 
    public string IENV_UUID { get; set; }// This is the invoice UUID, it should be unique for each invoice (required) 
    public string IENV_DATE { get; set; } // This is the invoice date, the format is yyyy-MM-dd (required)
    public string IENV_NOTE { get; set; } // This is the invoice note, it can be empty
    public string IENV_CUSTOMER_COMMERCIAL_NUMBER { get; set; } // This is the customer commercial number
    public string IENV_COMPANY_NAME { get; set; } // This is the company name, it should be the same as the one in the JoFotara dashboard (required)
    public string IENV_COMMERCIAL_NUMBER { get; set; }// This is the company commercial number, it should be the same as the one in the JoFotara dashboard (required)
    public string IENV_POSTAL_CODE { get; set; } // This is the postal code of the company, it should be the same as the one in the JoFotara docs (required)
    public string IENV_FAWTARA_CITY_NAME { get; set; }// This is the city name of the company, it should be the same as the one in the JoFotara docs (required)
    public string IENV_COMPANY_COMMERCIAL_NUMBER { get; set; } // This is the company commercial number, it should be the same as the one in the JoFotara dashboard (required)
    public string IENV_CUSTOMER_NAME { get; set; }// This is the customer name
    public string IENV_CUSTOMER_PHONE { get; set; }// This is the customer phone number
    public string IENV_Activity_Number { get; set; }// This is the activity number, it should be the same as the one in the JoFotara dashboard (required)
    public string IENV_TOTAL_AMT_WITHOUT_DISCOUNT { get; set; } // This is the total amount without discount
    public string IENV_TOTAL_AMT_WITH_DISCOUNT { get; set; } // This is the total amount with discount
    public string IENV_TOTAL_INVOICE { get; set; } // This is the total invoice amount
}



/// <summary>
/// This class represents the details of each line item in the invoice for JoFotara integration. (<cac:InvoiceLine> .. </cac:InvoiceLine>) Tag
/// </summary>
public class IENV_InvoiceLineDetails
{
    public string IENV_ID { get; set; } // This is the line item ID, it should be unique for each line item (required)
    public string IENV_PCE { get; set; }// This is the line item Quantity
    public string TOTAL_AMT_WITH_DISCOUNT { get; set; } // This is the total amount with discount for the line item
    public string INEV_ITEM_NAME { get; set; } // This is the line item name (required)
    public string IENV_PRICE_AMOUNT { get; set; }// This is the line item price amount (required)
    public string IENV_ROUNDING_AMOUNT { get; set; } // This is the rounding amount for the line item, it should be a positive number (required)
    public string IENV_TOTAL_LINE_EXTENSION // This is the total line extension amount, it should be a positive number (required)
    {
        get
        {
            var _TotalPrice = IENV_ROUNDING_AMOUNT;
            return _TotalPrice;
        }
    }
}
