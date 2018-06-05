 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.DALMSSQL
{
	public partial class AdminUserDAL : BaseDAL<Model.Entites.AdminUser>,IDAL.IAdminUserDAL
    {
    }
	public partial class CarInfoDAL : BaseDAL<Model.Entites.CarInfo>,IDAL.ICarInfoDAL
    {
    }
	public partial class HomeDataDAL : BaseDAL<Model.Entites.HomeData>,IDAL.IHomeDataDAL
    {
    }
	public partial class HouseCommentDAL : BaseDAL<Model.Entites.HouseComment>,IDAL.IHouseCommentDAL
    {
    }
	public partial class HouseConfigureDAL : BaseDAL<Model.Entites.HouseConfigure>,IDAL.IHouseConfigureDAL
    {
    }
	public partial class HouseEvaluateDAL : BaseDAL<Model.Entites.HouseEvaluate>,IDAL.IHouseEvaluateDAL
    {
    }
	public partial class HouseEvaluateViewDAL : BaseDAL<Model.Entites.HouseEvaluateView>,IDAL.IHouseEvaluateViewDAL
    {
    }
	public partial class HouseImgDAL : BaseDAL<Model.Entites.HouseImg>,IDAL.IHouseImgDAL
    {
    }
	public partial class HouseInfoDAL : BaseDAL<Model.Entites.HouseInfo>,IDAL.IHouseInfoDAL
    {
    }
	public partial class HouseInfoViewDAL : BaseDAL<Model.Entites.HouseInfoView>,IDAL.IHouseInfoViewDAL
    {
    }
	public partial class OrderInfoDAL : BaseDAL<Model.Entites.OrderInfo>,IDAL.IOrderInfoDAL
    {
    }
	public partial class OrderInfoViewDAL : BaseDAL<Model.Entites.OrderInfoView>,IDAL.IOrderInfoViewDAL
    {
    }
	public partial class OrderItemDAL : BaseDAL<Model.Entites.OrderItem>,IDAL.IOrderItemDAL
    {
    }
	public partial class ProductCategoryDAL : BaseDAL<Model.Entites.ProductCategory>,IDAL.IProductCategoryDAL
    {
    }
	public partial class ProductInfoDAL : BaseDAL<Model.Entites.ProductInfo>,IDAL.IProductInfoDAL
    {
    }
	public partial class RegionDAL : BaseDAL<Model.Entites.Region>,IDAL.IRegionDAL
    {
    }
	public partial class ShopCategoryDAL : BaseDAL<Model.Entites.ShopCategory>,IDAL.IShopCategoryDAL
    {
    }
	public partial class ShopInfoDAL : BaseDAL<Model.Entites.ShopInfo>,IDAL.IShopInfoDAL
    {
    }
	public partial class ShopInfoCertificateDAL : BaseDAL<Model.Entites.ShopInfoCertificate>,IDAL.IShopInfoCertificateDAL
    {
    }
	public partial class ShopInfoCertificateViewDAL : BaseDAL<Model.Entites.ShopInfoCertificateView>,IDAL.IShopInfoCertificateViewDAL
    {
    }
	public partial class SpotInfoDAL : BaseDAL<Model.Entites.SpotInfo>,IDAL.ISpotInfoDAL
    {
    }
	public partial class SysLogDAL : BaseDAL<Model.Entites.SysLog>,IDAL.ISysLogDAL
    {
    }
	public partial class SystemConfigDAL : BaseDAL<Model.Entites.SystemConfig>,IDAL.ISystemConfigDAL
    {
    }
	public partial class TenantInfoDAL : BaseDAL<Model.Entites.TenantInfo>,IDAL.ITenantInfoDAL
    {
    }
	public partial class UserAccountDAL : BaseDAL<Model.Entites.UserAccount>,IDAL.IUserAccountDAL
    {
    }
	public partial class UserFavoriteDAL : BaseDAL<Model.Entites.UserFavorite>,IDAL.IUserFavoriteDAL
    {
    }
	public partial class UserFavoriteViewDAL : BaseDAL<Model.Entites.UserFavoriteView>,IDAL.IUserFavoriteViewDAL
    {
    }
	public partial class UserInfoDAL : BaseDAL<Model.Entites.UserInfo>,IDAL.IUserInfoDAL
    {
    }
	public partial class UserInfoCertificateDAL : BaseDAL<Model.Entites.UserInfoCertificate>,IDAL.IUserInfoCertificateDAL
    {
    }
	public partial class UserInfoCertificateViewDAL : BaseDAL<Model.Entites.UserInfoCertificateView>,IDAL.IUserInfoCertificateViewDAL
    {
    }
	public partial class UserInfoExtDAL : BaseDAL<Model.Entites.UserInfoExt>,IDAL.IUserInfoExtDAL
    {
    }
	public partial class UserInfoExtViewDAL : BaseDAL<Model.Entites.UserInfoExtView>,IDAL.IUserInfoExtViewDAL
    {
    }
	public partial class UserInfoViewDAL : BaseDAL<Model.Entites.UserInfoView>,IDAL.IUserInfoViewDAL
    {
    }
	public partial class UserOrderRecordDAL : BaseDAL<Model.Entites.UserOrderRecord>,IDAL.IUserOrderRecordDAL
    {
    }
    public partial class UserDAL : BaseDAL<Model.Entites.User>, IDAL.IUserDAL
    {
    }
    public partial class PermissionDAL : BaseDAL<Model.Entites.Permission>, IDAL.IPermissionDAL
    {
    }
    public partial class RoleDAL : BaseDAL<Model.Entites.Role>, IDAL.IRoleDAL
    {
    }
    public partial class FunctionDAL:BaseDAL<Model.Entites.Function>,IDAL.IFunctionDAL
    {

    }
    public partial class FuncPermissionDAL : BaseDAL<Model.Entites.FuncPermission>, IDAL.IFuncPermissionDAL
    {
    }
    public partial class ShopToDayPriceSetDAL : BaseDAL<Model.Entites.ShopToDayPrice>, IDAL.IShopToDayPriceSetDAL
    {
    }
    public partial class UserRoleDAL:BaseDAL<Model.Entites.UserRole>,IDAL.IUserRoleDAL
    {

    }
}