using AutoMapper;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers {

    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase {

        private ICourseLibraryRepository _courseLibraryRepository;
        private IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper) {
            _courseLibraryRepository = courseLibraryRepository ?? throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors(
            [FromQuery] AuthorsResourceParameters authorsResourceParameters) {
            var authorsFromRepo = _courseLibraryRepository.GetAuthors(authorsResourceParameters);
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
        }

        [HttpGet("{authorId:guid}", Name = "GetAuthor")]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId) {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null) {
                return NotFound();
            }
            else {
                return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
            }
        }

        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto authorForCreationDto) {

            var authorEntity = _mapper.Map<Entities.Author>(authorForCreationDto);
            _courseLibraryRepository.AddAuthor(authorEntity);
            _courseLibraryRepository.Save();

            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthor",
                new { authorId = authorToReturn.Id },
                authorToReturn);

        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions() {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            Response.Headers.Add("ContentType", "JSON");

            return Ok();
        }
    }
}
