using CourseLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Models {
    [CourseTitleMustBeDifferentFromDescriptionAttribute(
        ErrorMessage = "Title must be different from description")]
    public abstract class CourseForManipulationDto {

        [Required(ErrorMessage = "You shoudl fill out the title")]
        [MaxLength(100, ErrorMessage = "The title souldn't have more than 100 characters.")]
        public string Title { get; set; }

        [MaxLength(1500, ErrorMessage = "The description shouldn't have more than 1500 characters.")]
        public virtual string Description { get; set; }
    }
}
