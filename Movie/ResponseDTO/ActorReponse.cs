using Movie.RequestDTO;
using System.Collections.Generic;

namespace Movie.ResponseDTO
{
    public class ActorReponse
    {
        public int ActorId { get; set; }
        public string ActorName { get; set; }
        public string ActorNational { get; set; }
    }

    public class ActorMovieDTO
    {
        public int MovieId { get; set; }
        public string AvatarUrl { get; set; }
        public string MovieName { get; set; }
    }

    public class ActorDetailDTO
    {
        public RequestActorDTO Actor { get; set; }
        public List<ActorMovieDTO> Movie { get; set; }
    }

}
