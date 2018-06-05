
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Holidays.IDAL;

namespace Holidays.DALMSSQL
{
	public partial class DBSession:IDAL.IDBSession
    {
		#region 01 数据接口 IAdminUserDAL
		IAdminUserDAL iAdminUserDAL;
		public IAdminUserDAL IAdminUserDAL
		{
			get
			{
				if(iAdminUserDAL==null)
					iAdminUserDAL= new AdminUserDAL();
				return iAdminUserDAL;
			}
			set
			{
				iAdminUserDAL= value;
			}
		}
		#endregion
		#region 02 数据接口 ICarInfoDAL
		ICarInfoDAL iCarInfoDAL;
		public ICarInfoDAL ICarInfoDAL
		{
			get
			{
				if(iCarInfoDAL==null)
					iCarInfoDAL= new CarInfoDAL();
				return iCarInfoDAL;
			}
			set
			{
				iCarInfoDAL= value;
			}
		}
		#endregion
		#region 03 数据接口 IHomeDataDAL
		IHomeDataDAL iHomeDataDAL;
		public IHomeDataDAL IHomeDataDAL
		{
			get
			{
				if(iHomeDataDAL==null)
					iHomeDataDAL= new HomeDataDAL();
				return iHomeDataDAL;
			}
			set
			{
				iHomeDataDAL= value;
			}
		}
		#endregion
		#region 04 数据接口 IHouseCommentDAL
		IHouseCommentDAL iHouseCommentDAL;
		public IHouseCommentDAL IHouseCommentDAL
		{
			get
			{
				if(iHouseCommentDAL==null)
					iHouseCommentDAL= new HouseCommentDAL();
				return iHouseCommentDAL;
			}
			set
			{
				iHouseCommentDAL= value;
			}
		}
		#endregion
		#region 05 数据接口 IHouseConfigureDAL
		IHouseConfigureDAL iHouseConfigureDAL;
		public IHouseConfigureDAL IHouseConfigureDAL
		{
			get
			{
				if(iHouseConfigureDAL==null)
					iHouseConfigureDAL= new HouseConfigureDAL();
				return iHouseConfigureDAL;
			}
			set
			{
				iHouseConfigureDAL= value;
			}
		}
		#endregion
		#region 06 数据接口 IHouseEvaluateDAL
		IHouseEvaluateDAL iHouseEvaluateDAL;
		public IHouseEvaluateDAL IHouseEvaluateDAL
		{
			get
			{
				if(iHouseEvaluateDAL==null)
					iHouseEvaluateDAL= new HouseEvaluateDAL();
				return iHouseEvaluateDAL;
			}
			set
			{
				iHouseEvaluateDAL= value;
			}
		}
		#endregion
		#region 07 数据接口 IHouseEvaluateViewDAL
		IHouseEvaluateViewDAL iHouseEvaluateViewDAL;
		public IHouseEvaluateViewDAL IHouseEvaluateViewDAL
		{
			get
			{
				if(iHouseEvaluateViewDAL==null)
					iHouseEvaluateViewDAL= new HouseEvaluateViewDAL();
				return iHouseEvaluateViewDAL;
			}
			set
			{
				iHouseEvaluateViewDAL= value;
			}
		}
		#endregion
		#region 08 数据接口 IHouseImgDAL
		IHouseImgDAL iHouseImgDAL;
		public IHouseImgDAL IHouseImgDAL
		{
			get
			{
				if(iHouseImgDAL==null)
					iHouseImgDAL= new HouseImgDAL();
				return iHouseImgDAL;
			}
			set
			{
				iHouseImgDAL= value;
			}
		}
		#endregion
		#region 09 数据接口 IHouseInfoDAL
		IHouseInfoDAL iHouseInfoDAL;
		public IHouseInfoDAL IHouseInfoDAL
		{
			get
			{
				if(iHouseInfoDAL==null)
					iHouseInfoDAL= new HouseInfoDAL();
				return iHouseInfoDAL;
			}
			set
			{
				iHouseInfoDAL= value;
			}
		}
		#endregion
		#region 10 数据接口 IHouseInfoViewDAL
		IHouseInfoViewDAL iHouseInfoViewDAL;
		public IHouseInfoViewDAL IHouseInfoViewDAL
		{
			get
			{
				if(iHouseInfoViewDAL==null)
					iHouseInfoViewDAL= new HouseInfoViewDAL();
				return iHouseInfoViewDAL;
			}
			set
			{
				iHouseInfoViewDAL= value;
			}
		}
		#endregion
		#region 11 数据接口 IOrderInfoDAL
		IOrderInfoDAL iOrderInfoDAL;
		public IOrderInfoDAL IOrderInfoDAL
		{
			get
			{
				if(iOrderInfoDAL==null)
					iOrderInfoDAL= new OrderInfoDAL();
				return iOrderInfoDAL;
			}
			set
			{
				iOrderInfoDAL= value;
			}
		}
		#endregion
		#region 12 数据接口 IOrderInfoViewDAL
		IOrderInfoViewDAL iOrderInfoViewDAL;
		public IOrderInfoViewDAL IOrderInfoViewDAL
		{
			get
			{
				if(iOrderInfoViewDAL==null)
					iOrderInfoViewDAL= new OrderInfoViewDAL();
				return iOrderInfoViewDAL;
			}
			set
			{
				iOrderInfoViewDAL= value;
			}
		}
		#endregion
		#region 13 数据接口 IOrderItemDAL
		IOrderItemDAL iOrderItemDAL;
		public IOrderItemDAL IOrderItemDAL
		{
			get
			{
				if(iOrderItemDAL==null)
					iOrderItemDAL= new OrderItemDAL();
				return iOrderItemDAL;
			}
			set
			{
				iOrderItemDAL= value;
			}
		}
		#endregion
		#region 14 数据接口 IProductCategoryDAL
		IProductCategoryDAL iProductCategoryDAL;
		public IProductCategoryDAL IProductCategoryDAL
		{
			get
			{
				if(iProductCategoryDAL==null)
					iProductCategoryDAL= new ProductCategoryDAL();
				return iProductCategoryDAL;
			}
			set
			{
				iProductCategoryDAL= value;
			}
		}
		#endregion
		#region 15 数据接口 IProductInfoDAL
		IProductInfoDAL iProductInfoDAL;
		public IProductInfoDAL IProductInfoDAL
		{
			get
			{
				if(iProductInfoDAL==null)
					iProductInfoDAL= new ProductInfoDAL();
				return iProductInfoDAL;
			}
			set
			{
				iProductInfoDAL= value;
			}
		}
		#endregion
		#region 16 数据接口 IRegionDAL
		IRegionDAL iRegionDAL;
		public IRegionDAL IRegionDAL
		{
			get
			{
				if(iRegionDAL==null)
					iRegionDAL= new RegionDAL();
				return iRegionDAL;
			}
			set
			{
				iRegionDAL= value;
			}
		}
		#endregion
		#region 17 数据接口 IShopCategoryDAL
		IShopCategoryDAL iShopCategoryDAL;
		public IShopCategoryDAL IShopCategoryDAL
		{
			get
			{
				if(iShopCategoryDAL==null)
					iShopCategoryDAL= new ShopCategoryDAL();
				return iShopCategoryDAL;
			}
			set
			{
				iShopCategoryDAL= value;
			}
		}
		#endregion
		#region 18 数据接口 IShopInfoDAL
		IShopInfoDAL iShopInfoDAL;
		public IShopInfoDAL IShopInfoDAL
		{
			get
			{
				if(iShopInfoDAL==null)
					iShopInfoDAL= new ShopInfoDAL();
				return iShopInfoDAL;
			}
			set
			{
				iShopInfoDAL= value;
			}
		}
		#endregion
		#region 19 数据接口 IShopInfoCertificateDAL
		IShopInfoCertificateDAL iShopInfoCertificateDAL;
		public IShopInfoCertificateDAL IShopInfoCertificateDAL
		{
			get
			{
				if(iShopInfoCertificateDAL==null)
					iShopInfoCertificateDAL= new ShopInfoCertificateDAL();
				return iShopInfoCertificateDAL;
			}
			set
			{
				iShopInfoCertificateDAL= value;
			}
		}
		#endregion
		#region 20 数据接口 IShopInfoCertificateViewDAL
		IShopInfoCertificateViewDAL iShopInfoCertificateViewDAL;
		public IShopInfoCertificateViewDAL IShopInfoCertificateViewDAL
		{
			get
			{
				if(iShopInfoCertificateViewDAL==null)
					iShopInfoCertificateViewDAL= new ShopInfoCertificateViewDAL();
				return iShopInfoCertificateViewDAL;
			}
			set
			{
				iShopInfoCertificateViewDAL= value;
			}
		}
		#endregion
		#region 21 数据接口 ISpotInfoDAL
		ISpotInfoDAL iSpotInfoDAL;
		public ISpotInfoDAL ISpotInfoDAL
		{
			get
			{
				if(iSpotInfoDAL==null)
					iSpotInfoDAL= new SpotInfoDAL();
				return iSpotInfoDAL;
			}
			set
			{
				iSpotInfoDAL= value;
			}
		}
		#endregion
		#region 22 数据接口 ISysLogDAL
		ISysLogDAL iSysLogDAL;
		public ISysLogDAL ISysLogDAL
		{
			get
			{
				if(iSysLogDAL==null)
					iSysLogDAL= new SysLogDAL();
				return iSysLogDAL;
			}
			set
			{
				iSysLogDAL= value;
			}
		}
		#endregion
		#region 23 数据接口 ISystemConfigDAL
		ISystemConfigDAL iSystemConfigDAL;
		public ISystemConfigDAL ISystemConfigDAL
		{
			get
			{
				if(iSystemConfigDAL==null)
					iSystemConfigDAL= new SystemConfigDAL();
				return iSystemConfigDAL;
			}
			set
			{
				iSystemConfigDAL= value;
			}
		}
		#endregion
		#region 24 数据接口 ITenantInfoDAL
		ITenantInfoDAL iTenantInfoDAL;
		public ITenantInfoDAL ITenantInfoDAL
		{
			get
			{
				if(iTenantInfoDAL==null)
					iTenantInfoDAL= new TenantInfoDAL();
				return iTenantInfoDAL;
			}
			set
			{
				iTenantInfoDAL= value;
			}
		}
		#endregion
		#region 25 数据接口 IUserAccountDAL
		IUserAccountDAL iUserAccountDAL;
		public IUserAccountDAL IUserAccountDAL
		{
			get
			{
				if(iUserAccountDAL==null)
					iUserAccountDAL= new UserAccountDAL();
				return iUserAccountDAL;
			}
			set
			{
				iUserAccountDAL= value;
			}
		}
		#endregion
		#region 26 数据接口 IUserFavoriteDAL
		IUserFavoriteDAL iUserFavoriteDAL;
		public IUserFavoriteDAL IUserFavoriteDAL
		{
			get
			{
				if(iUserFavoriteDAL==null)
					iUserFavoriteDAL= new UserFavoriteDAL();
				return iUserFavoriteDAL;
			}
			set
			{
				iUserFavoriteDAL= value;
			}
		}
		#endregion
		#region 27 数据接口 IUserFavoriteViewDAL
		IUserFavoriteViewDAL iUserFavoriteViewDAL;
		public IUserFavoriteViewDAL IUserFavoriteViewDAL
		{
			get
			{
				if(iUserFavoriteViewDAL==null)
					iUserFavoriteViewDAL= new UserFavoriteViewDAL();
				return iUserFavoriteViewDAL;
			}
			set
			{
				iUserFavoriteViewDAL= value;
			}
		}
		#endregion
		#region 28 数据接口 IUserInfoDAL
		IUserInfoDAL iUserInfoDAL;
		public IUserInfoDAL IUserInfoDAL
		{
			get
			{
				if(iUserInfoDAL==null)
					iUserInfoDAL= new UserInfoDAL();
				return iUserInfoDAL;
			}
			set
			{
				iUserInfoDAL= value;
			}
		}
		#endregion
		#region 29 数据接口 IUserInfoCertificateDAL
		IUserInfoCertificateDAL iUserInfoCertificateDAL;
		public IUserInfoCertificateDAL IUserInfoCertificateDAL
		{
			get
			{
				if(iUserInfoCertificateDAL==null)
					iUserInfoCertificateDAL= new UserInfoCertificateDAL();
				return iUserInfoCertificateDAL;
			}
			set
			{
				iUserInfoCertificateDAL= value;
			}
		}
		#endregion
		#region 30 数据接口 IUserInfoCertificateViewDAL
		IUserInfoCertificateViewDAL iUserInfoCertificateViewDAL;
		public IUserInfoCertificateViewDAL IUserInfoCertificateViewDAL
		{
			get
			{
				if(iUserInfoCertificateViewDAL==null)
					iUserInfoCertificateViewDAL= new UserInfoCertificateViewDAL();
				return iUserInfoCertificateViewDAL;
			}
			set
			{
				iUserInfoCertificateViewDAL= value;
			}
		}
		#endregion
		#region 31 数据接口 IUserInfoExtDAL
		IUserInfoExtDAL iUserInfoExtDAL;
		public IUserInfoExtDAL IUserInfoExtDAL
		{
			get
			{
				if(iUserInfoExtDAL==null)
					iUserInfoExtDAL= new UserInfoExtDAL();
				return iUserInfoExtDAL;
			}
			set
			{
				iUserInfoExtDAL= value;
			}
		}
		#endregion
		#region 32 数据接口 IUserInfoExtViewDAL
		IUserInfoExtViewDAL iUserInfoExtViewDAL;
		public IUserInfoExtViewDAL IUserInfoExtViewDAL
		{
			get
			{
				if(iUserInfoExtViewDAL==null)
					iUserInfoExtViewDAL= new UserInfoExtViewDAL();
				return iUserInfoExtViewDAL;
			}
			set
			{
				iUserInfoExtViewDAL= value;
			}
		}
		#endregion
		#region 33 数据接口 IUserInfoViewDAL
		IUserInfoViewDAL iUserInfoViewDAL;
		public IUserInfoViewDAL IUserInfoViewDAL
		{
			get
			{
				if(iUserInfoViewDAL==null)
					iUserInfoViewDAL= new UserInfoViewDAL();
				return iUserInfoViewDAL;
			}
			set
			{
				iUserInfoViewDAL= value;
			}
		}
		#endregion
		#region 34 数据接口 IUserOrderRecordDAL
		IUserOrderRecordDAL iUserOrderRecordDAL;
		public IUserOrderRecordDAL IUserOrderRecordDAL
		{
			get
			{
				if(iUserOrderRecordDAL==null)
					iUserOrderRecordDAL= new UserOrderRecordDAL();
				return iUserOrderRecordDAL;
			}
			set
			{
				iUserOrderRecordDAL= value;
			}
		}
        #endregion
        #region IUserDAL
        IUserDAL iUserDAL;
        public IUserDAL IUserDAL
        {
            get
            {
                if (iUserDAL == null)
                {
                    iUserDAL = new UserDAL();
                }
                return iUserDAL;
            }
            set
            {
                iUserDAL = value;
            }
        }
        #endregion
        #region iPermissionDAL
        IPermissionDAL iPermissionDAL;
        public IPermissionDAL IPermissionDAL
        {
            get
            {
                if (iPermissionDAL == null)
                {
                    iPermissionDAL = new PermissionDAL();
                }
                return iPermissionDAL;
            }
            set
            {
                iPermissionDAL = value;
            }
        }
        #endregion
        #region iRoleDAL
        IRoleDAL iRoleDAL;
        public IRoleDAL IRoleDAL
        {
            get
            {
                if (iRoleDAL == null)
                {
                    iRoleDAL = new RoleDAL();
                }
                return iRoleDAL;
            }
            set
            {
                iRoleDAL = value;
            }
        }
        #endregion
        #region IFunctionDAL
        IFunctionDAL iFunctionDAL;
        public IFunctionDAL IFunctionDAL
        {
            get
            {
                if (iFunctionDAL == null)
                {
                    iFunctionDAL = new FunctionDAL();
                }
                return iFunctionDAL;
            }
            set
            {
                iFunctionDAL = value;
            }
        }
        #endregion
        #region IFuncPermissionDAL
        IFuncPermissionDAL iFuncPermissionDAL;
        public IFuncPermissionDAL IFuncPermissionDAL
        {
            get
            {
                if (iFuncPermissionDAL == null)
                {
                    iFuncPermissionDAL = new FuncPermissionDAL();
                }
                return iFuncPermissionDAL;
            }
            set
            {
                iFuncPermissionDAL = value;
            }
        }
        #endregion
        #region IUserRoleDAL
        IUserRoleDAL iUserRoleDAL;
        public IUserRoleDAL IUserRoleDAL
        {
            get
            {
                if (iUserRoleDAL == null)
                {
                    iUserRoleDAL = new UserRoleDAL();
                }
                return iUserRoleDAL;
            }
            set
            {
                iUserRoleDAL = value;
            }
        }
        #endregion
        
        #region iShopToDayPriceSetDAL
        IShopToDayPriceSetDAL iShopToDayPriceSetDAL;
        public IShopToDayPriceSetDAL IShopToDayPriceSetDAL
        {
            get
            {
                if (iShopToDayPriceSetDAL == null)
                {
                    iShopToDayPriceSetDAL = new ShopToDayPriceSetDAL();
                }
                return iShopToDayPriceSetDAL;
            }
            set
            {
                iShopToDayPriceSetDAL = value;
            }
        }
        #endregion
    }
}