namespace TraineeManagementApi.DTO.Pagination;

public class PageResponse<T>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 2;
    public int TotalRecords { get; init; } = 0;
    public T? Data { get; init; }
}