using Movie.RequestDTO;

public interface IDirectorsRepository
{
    Task<DirectorDetailDTO?> GetDirectorByIdAsync(int id);
    Task<IEnumerable<RequestDirectorDTO>> GetAllDirectorsAsync(
        string? search = null,
        string sortBy = "NameDir",
        string sortDirection = "asc",
        int page = 1,
        int pageSize = 10
    );
    Task<RequestDirectorDTO> UpdateDirectorAsync(int id, RequestDirectorDTO directorDTO);
    Task<RequestDirectorDTO> AddDirectorAsync(RequestDirectorDTO directorDTO, IFormFile AvatarUrlFile);
}
