using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entities
{
    /// <summary>
    /// Khách hàng
    /// NVTOAN 24/06/2021
    /// </summary>
    public class Customer : BaseEntity
    {

        #region property

        /// <summary>
        /// Khóa chính
        /// </summary>
        [PrimaryKey]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        [Required]
        [Unique]
        [DisplayName("Mã khách hàng")]
        public string CustomerCode { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        [DisplayName("Tên khách hàng")]
        public string FullName { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [DisplayName("Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [DisplayName("Địa chỉ")]
        public string Adress { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [Required]
        [Unique]
        [DisplayName("Số điện thoại")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Giới tính (0-Nữ, 1-Nam, 2-Khác)
        /// </summary>
        [DisplayName("Giới tính")]
        public int? Gender { get; set; }

        /// <summary>
        /// Email khách hàng
        /// </summary>
        [Required]
        [Unique]
        [DisplayName("Email")]
        public string Email { get; set; }

        /// <summary>
        /// Khóa ngoại đến nhóm khách hàng
        /// </summary>
        [ForeignKey]
        public Guid? CustomerGroupId { get; set; }

        /// <summary>
        /// Tên nhóm khách hàng
        /// </summary>
        [DisplayName("Nhóm khách hàng")]
        public string CustomerGroupName { get; set; }

        /// <summary>
        /// Số thẻ thành viên
        /// </summary>
        [DisplayName("Mã thẻ thành viên")]
        public string MemberCardCode { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        [DisplayName("Ghi chú")]
        public string Note { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        [DisplayName("Tên công ty")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Mã số thuế công ty
        /// </summary>
        [DisplayName("Mã số thuế")]
        public string CompanyTaxCode { get; set; }

        #endregion
    }
}
