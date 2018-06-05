using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holidays.BLL
{
    public partial class AdminUserBLL : BaseBLL<Model.Entites.AdminUser>, IBLL.IAdminUserBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IAdminUserDAL;
        }
    }

    public partial class CarInfoBLL : BaseBLL<Model.Entites.CarInfo>, IBLL.ICarInfoBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.ICarInfoDAL;
        }
    }

    public partial class HomeDataBLL : BaseBLL<Model.Entites.HomeData>, IBLL.IHomeDataBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IHomeDataDAL;
        }
    }

    public partial class HouseCommentBLL : BaseBLL<Model.Entites.HouseComment>, IBLL.IHouseCommentBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IHouseCommentDAL;
        }
    }

    public partial class HouseConfigureBLL : BaseBLL<Model.Entites.HouseConfigure>, IBLL.IHouseConfigureBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IHouseConfigureDAL;
        }
    }

    public partial class HouseEvaluateBLL : BaseBLL<Model.Entites.HouseEvaluate>, IBLL.IHouseEvaluateBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IHouseEvaluateDAL;
        }
    }

    public partial class HouseEvaluateViewBLL : BaseBLL<Model.Entites.HouseEvaluateView>, IBLL.IHouseEvaluateViewBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IHouseEvaluateViewDAL;
        }
    }

    public partial class HouseImgBLL : BaseBLL<Model.Entites.HouseImg>, IBLL.IHouseImgBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IHouseImgDAL;
        }
    }

    public partial class HouseInfoBLL : BaseBLL<Model.Entites.HouseInfo>, IBLL.IHouseInfoBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IHouseInfoDAL;
        }
    }

    public partial class HouseInfoViewBLL : BaseBLL<Model.Entites.HouseInfoView>, IBLL.IHouseInfoViewBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IHouseInfoViewDAL;
        }
    }

    public partial class OrderInfoBLL : BaseBLL<Model.Entites.OrderInfo>, IBLL.IOrderInfoBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IOrderInfoDAL;
        }
    }

    public partial class OrderInfoViewBLL : BaseBLL<Model.Entites.OrderInfoView>, IBLL.IOrderInfoViewBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IOrderInfoViewDAL;
        }
    }

    public partial class OrderItemBLL : BaseBLL<Model.Entites.OrderItem>, IBLL.IOrderItemBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IOrderItemDAL;
        }
    }

    public partial class ProductCategoryBLL : BaseBLL<Model.Entites.ProductCategory>, IBLL.IProductCategoryBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IProductCategoryDAL;
        }
    }

    public partial class ProductInfoBLL : BaseBLL<Model.Entites.ProductInfo>, IBLL.IProductInfoBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IProductInfoDAL;
        }
    }

    public partial class RegionBLL : BaseBLL<Model.Entites.Region>, IBLL.IRegionBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IRegionDAL;
        }
    }

    public partial class ShopCategoryBLL : BaseBLL<Model.Entites.ShopCategory>, IBLL.IShopCategoryBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IShopCategoryDAL;
        }
    }
    public partial class ShopToDayPriceSetBLL : BaseBLL<Model.Entites.ShopToDayPrice>, IBLL.IShopToDaySetBll
    {
        public override void SetDAL()
        {
            idal = DBSession.IShopToDayPriceSetDAL;
        }
    }

    public partial class ShopInfoBLL : BaseBLL<Model.Entites.ShopInfo>, IBLL.IShopInfoBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IShopInfoDAL;
        }
    }

    public partial class ShopInfoCertificateBLL : BaseBLL<Model.Entites.ShopInfoCertificate>, IBLL.IShopInfoCertificateBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IShopInfoCertificateDAL;
        }
    }

    public partial class ShopInfoCertificateViewBLL : BaseBLL<Model.Entites.ShopInfoCertificateView>, IBLL.IShopInfoCertificateViewBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IShopInfoCertificateViewDAL;
        }
    }

    public partial class SpotInfoBLL : BaseBLL<Model.Entites.SpotInfo>, IBLL.ISpotInfoBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.ISpotInfoDAL;
        }
    }

    public partial class SysLogBLL : BaseBLL<Model.Entites.SysLog>, IBLL.ISysLogBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.ISysLogDAL;
        }
    }

    public partial class SystemConfigBLL : BaseBLL<Model.Entites.SystemConfig>, IBLL.ISystemConfigBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.ISystemConfigDAL;
        }
    }

    public partial class TenantInfoBLL : BaseBLL<Model.Entites.TenantInfo>, IBLL.ITenantInfoBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.ITenantInfoDAL;
        }
    }

    public partial class UserAccountBLL : BaseBLL<Model.Entites.UserAccount>, IBLL.IUserAccountBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IUserAccountDAL;
        }
    }

    public partial class UserFavoriteBLL : BaseBLL<Model.Entites.UserFavorite>, IBLL.IUserFavoriteBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IUserFavoriteDAL;
        }
    }

    public partial class UserFavoriteViewBLL : BaseBLL<Model.Entites.UserFavoriteView>, IBLL.IUserFavoriteViewBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IUserFavoriteViewDAL;
        }
    }

    public partial class UserInfoBLL : BaseBLL<Model.Entites.UserInfo>, IBLL.IUserInfoBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IUserInfoDAL;
        }
    }

    public partial class UserInfoCertificateBLL : BaseBLL<Model.Entites.UserInfoCertificate>, IBLL.IUserInfoCertificateBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IUserInfoCertificateDAL;
        }
    }

    public partial class UserInfoCertificateViewBLL : BaseBLL<Model.Entites.UserInfoCertificateView>, IBLL.IUserInfoCertificateViewBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IUserInfoCertificateViewDAL;
        }
    }

    public partial class UserInfoExtBLL : BaseBLL<Model.Entites.UserInfoExt>, IBLL.IUserInfoExtBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IUserInfoExtDAL;
        }
    }

    public partial class UserInfoExtViewBLL : BaseBLL<Model.Entites.UserInfoExtView>, IBLL.IUserInfoExtViewBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IUserInfoExtViewDAL;
        }
    }

    public partial class UserInfoViewBLL : BaseBLL<Model.Entites.UserInfoView>, IBLL.IUserInfoViewBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IUserInfoViewDAL;
        }
    }

    public partial class UserOrderRecordBLL : BaseBLL<Model.Entites.UserOrderRecord>, IBLL.IUserOrderRecordBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IUserOrderRecordDAL;
        }
    }
    public partial class UserBLL : BaseBLL<Model.Entites.User>, IBLL.IUserBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IUserDAL;

        }
    }
    public partial class PermissionBLL : BaseBLL<Model.Entites.Permission>, IBLL.IPermissionBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IPermissionDAL;

        }
    }

    public partial class RoleBLL : BaseBLL<Model.Entites.Role>, IBLL.IRoleBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IRoleDAL;

        }
    }
    public partial class FunctionBLL : BaseBLL<Model.Entites.Function>, IBLL.IFunctionBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IFunctionDAL;
        }
    }
    public partial class FuncPermissionBLL : BaseBLL<Model.Entites.FuncPermission>, IBLL.IFuncPermissionBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IFuncPermissionDAL;
        }
    }
    public partial class UserRoleBLL : BaseBLL<Model.Entites.UserRole>, IBLL.IUserRoleBLL
    {
        public override void SetDAL()
        {
            idal = DBSession.IUserRoleDAL;
        }
    }
}
