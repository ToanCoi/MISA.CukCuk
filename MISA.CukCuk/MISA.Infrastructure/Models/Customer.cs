using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Models
{
    /// <summary>
    /// Khách hàng
    /// NVTOAN 24/06/2021
    /// </summary>
    public class Customer
    {

        #region property

        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Adress { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Giới tính (0-Nữ, 1-Nam, 2-Khác)
        /// </summary>
        public int? Gender { get; set; }

        /// <summary>
        /// Email khách hàng
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }


        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Khóa ngoại đến nhóm khách hàng
        /// </summary>
        public Guid? CustomerGroupId { get; set; }

        /// <summary>
        /// Số thẻ thành viên
        /// </summary>
        public string MemberCardCode { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Mã số thuế công ty
        /// </summary>
        public string CompanyTaxCode { get; set; }

        #endregion
    }
}
