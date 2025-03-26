using Movie.RequestDTO;

public interface IDirectorsRepository
{
    Task<DirectorDetailDTO?> GetDirectorByIdAsync(int id);
    Task<IEnumerable<RequestDirectorDTO>> GetAllDirectorsAsync(
        string? search = null,
        string sortBy = "NameDir",
        string sortDirection = "asc",
        int page = 1,
        int pageSize = 5
    );
    Task<RequestDirectorDTO> AddDirectorAsync(RequestDirectorDTO directorDTO);
    Task<RequestDirectorDTO> UpdateDirectorAsync(int id, RequestDirectorDTO directorDTO);
}
