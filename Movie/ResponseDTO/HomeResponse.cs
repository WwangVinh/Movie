using System.Collections.Generic;
using Movie.RequestDTO;

namespace Movie.ResponseDTO
{
    public class HomeResponse
    {
        public IEnumerable<RequestDirectorDTO> PosterMovie { get; set; }
        public IEnumerable<RequestDirectorDTO> HotMovie { get; set; }
        public IEnumerable<RequestDirectorDTO> NewMovie { get; set; }
        public IEnumerable<RequestDirectorDTO> SeriesMovie { get; set; }
        public IEnumerable<RequestDirectorDTO> ActionMovie { get; set; }
    }
}
