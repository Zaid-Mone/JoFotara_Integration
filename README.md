# 🧾 JoFotara Integration API

This project is built with **ASP.NET Core 8 Web API ** and is designed to integrate with the **JoFotara** (Jordan's official e-invoicing system). It sends invoice data as an XML file encoded in Base64 to the JoFotara API endpoint.

## 🔍 Project Overview

- **Endpoint**: `/api/JoFotara/SendFotaraInvoiceV1`
- **Technologies**: ASP.NET Core, XML via LINQ to XML


## 
## 📁 Project Structure

```
JoFotara_Integration/
├── Controllers/
│   └── JoFotaraController.cs
├── DTOs/
│   └── (Includes DTOs like JoFotaraRequest, IENV_Fotara, IENV_InvoiceLineDetails)
├── wwwroot/
│   └── XML/
│       └── SendingFawtaraXML.xml
├── appsettings.json
├── Program.cs
```

## 📝 Note

- **REQUEST**: Please keep in mind to refer to the `DTOs/JoFotaraRequest.cs` class where I have set detailed notes.
- **RESPONSE**: Please see the `DTOs/EINV_Response.cs` class for the JoFotara API response structure.



## ⚙️ Configuration

Update the `appsettings.json` file with your JoFotara credentials and company info:

```json
"JoFotara": {
  "Client_Id": "your-client-id",
  "Secret_Key": "your-secret-key",
  "ApiUrl": "https://api.fotara.gov.jo/v1/invoices",
  "Activity_Number": "your-activity-number",
  "Company_Commercial_Number": "your-commercial-number",
  "Company_Name": "your-company-name"
}
```







## 🚀 Getting Started

1. **Clone the repository**:
   ```bash
   git clone https://github.com/your-org/JoFotara_Integration.git
   cd JoFotara_Integration
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Run the project**:
   ```bash
   dotnet run
   ```

4. **Access the API**:
   ```
   https://localhost:{port}/api/JoFotara/SendFotaraInvoiceV1
   ```

## 📨 API Endpoint: SendFotaraInvoiceV1

This endpoint generates a UBL-compliant XML invoice and sends it to the official JoFotara service.


## 🧾 Notes

- The XML format follows the UBL (Universal Business Language) schema.
- XML is created using `System.Xml.Linq` (LINQ to XML) and then encoded in Base64.
- The `wwwroot/XML/SendingFawtaraXML.xml` file contains a sample XML invoice.


## 📬 Contact

For questions or issues, please  open an issue on GitHub.

---

Built with ❤️ to simplify integration with Jordan’s official e-invoicing system, JoFotara.
