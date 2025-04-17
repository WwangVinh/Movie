using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Movie.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Movie.Service
{
    public class JwtService
    {
        private readonly IConfiguration _config;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(IConfiguration config)
        {
            _config = config;

            // Lấy các cấu hình từ appsettings.json
            _secretKey = _config.GetValue<string>("Jwt:Key");
            _issuer = _config.GetValue<string>("Jwt:Issuer");
            _audience = _config.GetValue<string>("Jwt:Audience");

            // Kiểm tra xem cấu hình đã đầy đủ chưa
            if (string.IsNullOrEmpty(_secretKey) || string.IsNullOrEmpty(_issuer) || string.IsNullOrEmpty(_audience))
            {
                throw new InvalidOperationException("Cấu hình Jwt trong appsettings.json chưa đầy đủ.");
            }
        }

        public string GenerateToken(User user)
        {
            // Kiểm tra người dùng có thông tin hợp lệ không
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Người dùng không hợp lệ.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // Tạo key từ secret key đã được lấy từ cấu hình
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            // Thiết lập các quyền ký
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tạo JWT token
            var token = new JwtSecurityToken(
                _issuer,  // Issuer
                _audience,  // Audience
                claims,  // Claims
                expires: DateTime.UtcNow.AddHours(3),  // Token sẽ hết hạn sau 3 giờ
                signingCredentials: creds  // Quyền ký
            );

            // Trả về token dưới dạng chuỗi
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
