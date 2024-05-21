using System.ComponentModel.DataAnnotations;

namespace DoctorBookManager.Data.EntityBase
{
    public interface IHasCreationAudited:IEntity
    {
        public DateTime CreationTime { get; set; }
        
        public string? CreatorUserId { get; set; }
    }
}
