namespace JoFotara_Integration.DTOs;






/// <summary> 
/// Demo Purpose:
/// You can use this class to store the JoFotara credentials, you can get them from the JoFotara dashboard.
/// I have saved the credentials in the appsettings.json file, you can also use environment variables or any other way to store them  like  db.
/// </summary>
public sealed class JoFotaraCredentials
{
    public string ApiUrl { get; set; }
    public string Client_Id { get; set; } //  you can get this from the JoFotara dashboard
    public string Secret_Key { get; set; } //  you can get this from the JoFotara dashboard
    public string Activity_Number { get; set; } //  you can get this from the JoFotara dashboard
    public string Company_Name { get; set; } //  you can get this from the JoFotara dashboard
    public string Company_Commercial_Number { get; set; } //  you can get this from the JoFotara dashboard
}
