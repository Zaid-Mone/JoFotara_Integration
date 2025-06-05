namespace JoFotara_Integration.DTOs;

public class EINV_Response
{
    public EINV_RESULTS EINV_RESULTS { get; set; }
    public string EINV_STATUS { get; set; }
    public string EINV_SINGED_INVOICE { get; set; }
    public string EINV_QR { get; set; }
    public string EINV_NUM { get; set; }
    public string EINV_INV_UUID { get; set; }
}




public class EINV_RESULTS
{
    public string status { get; set; }
    public List<EINV_INFO> INFO { get; set; }
    public List<object> WARNINGS { get; set; }
    public List<object> ERRORS { get; set; }
}

public class EINV_INFO
{
    public string Type { get; set; }
    public string Status { get; set; }
    public string EINV_CODE { get; set; }
    public string EINV_CATEGORY { get; set; }
    public string EINV_MESSAGE { get; set; }
}
