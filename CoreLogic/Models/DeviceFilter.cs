namespace CoreLogic.Models;

public class DeviceFilter
{
    public Guid? DepartmentId { get; set; }
    public int? StatusId { get; set; }
    public string? SearchText { get; set; }
    public string SortBy { get; set; } = "created";
    public bool SortDescending { get; set; } = true;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public void Validate()
    {
        if (Page < 1) Page = 1;
        if (PageSize < 1) PageSize = 1;
        if (PageSize > 100) PageSize = 100;
    }
}