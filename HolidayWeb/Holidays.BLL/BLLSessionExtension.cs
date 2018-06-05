
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Holidays.IBLL;

namespace Holidays.BLL
{
	public partial class BLLSession:IBLL.IBLLSession
    {
		#region 01 业务接口 IAdminUserDAL
		IAdminUserBLL iAdminUserBLL;
		public IAdminUserBLL IAdminUserBLL
		{
			get
			{
				if(iAdminUserBLL==null)
					iAdminUserBLL= new AdminUserBLL();
				return iAdminUserBLL;
			}
			set
			{
				iAdminUserBLL= value;
			}
		}
		#endregion

		#region 02 业务接口 ICarInfoDAL
		ICarInfoBLL iCarInfoBLL;
		public ICarInfoBLL ICarInfoBLL
		{
			get
			{
				if(iCarInfoBLL==null)
					iCarInfoBLL= new CarInfoBLL();
				return iCarInfoBLL;
			}
			set
			{
				iCarInfoBLL= value;
			}
		}
		#endregion

		#region 03 业务接口 IHomeDataDAL
		IHomeDataBLL iHomeDataBLL;
		public IHomeDataBLL IHomeDataBLL
		{
			get
			{
				if(iHomeDataBLL==null)
					iHomeDataBLL= new HomeDataBLL();
				return iHomeDataBLL;
			}
			set
			{
				iHomeDataBLL= value;
			}
		}
		#endregion

		#region 04 业务接口 IHouseCommentDAL
		IHouseCommentBLL iHouseCommentBLL;
		public IHouseCommentBLL IHouseCommentBLL
		{
			get
			{
				if(iHouseCommentBLL==null)
					iHouseCommentBLL= new HouseCommentBLL();
				return iHouseCommentBLL;
			}
			set
			{
				iHouseCommentBLL= value;
			}
		}
		#endregion

		#region 05 业务接口 IHouseConfigureDAL
		IHouseConfigureBLL iHouseConfigureBLL;
		public IHouseConfigureBLL IHouseConfigureBLL
		{
			get
			{
				if(iHouseConfigureBLL==null)
					iHouseConfigureBLL= new HouseConfigureBLL();
				return iHouseConfigureBLL;
			}
			set
			{
				iHouseConfigureBLL= value;
			}
		}
		#endregion

		#region 06 业务接口 IHouseEvaluateDAL
		IHouseEvaluateBLL iHouseEvaluateBLL;
		public IHouseEvaluateBLL IHouseEvaluateBLL
		{
			get
			{
				if(iHouseEvaluateBLL==null)
					iHouseEvaluateBLL= new HouseEvaluateBLL();
				return iHouseEvaluateBLL;
			}
			set
			{
				iHouseEvaluateBLL= value;
			}
		}
		#endregion

		#region 07 业务接口 IHouseEvaluateViewDAL
		IHouseEvaluateViewBLL iHouseEvaluateViewBLL;
		public IHouseEvaluateViewBLL IHouseEvaluateViewBLL
		{
			get
			{
				if(iHouseEvaluateViewBLL==null)
					iHouseEvaluateViewBLL= new HouseEvaluateViewBLL();
				return iHouseEvaluateViewBLL;
			}
			set
			{
				iHouseEvaluateViewBLL= value;
			}
		}
		#endregion

		#region 08 业务接口 IHouseImgDAL
		IHouseImgBLL iHouseImgBLL;
		public IHouseImgBLL IHouseImgBLL
		{
			get
			{
				if(iHouseImgBLL==null)
					iHouseImgBLL= new HouseImgBLL();
				return iHouseImgBLL;
			}
			set
			{
				iHouseImgBLL= value;
			}
		}
		#endregion

		#region 09 业务接口 IHouseInfoDAL
		IHouseInfoBLL iHouseInfoBLL;
		public IHouseInfoBLL IHouseInfoBLL
		{
			get
			{
				if(iHouseInfoBLL==null)
					iHouseInfoBLL= new HouseInfoBLL();
				return iHouseInfoBLL;
			}
			set
			{
				iHouseInfoBLL= value;
			}
		}
		#endregion

		#region 10 业务接口 IHouseInfoViewDAL
		IHouseInfoViewBLL iHouseInfoViewBLL;
		public IHouseInfoViewBLL IHouseInfoViewBLL
		{
			get
			{
				if(iHouseInfoViewBLL==null)
					iHouseInfoViewBLL= new HouseInfoViewBLL();
				return iHouseInfoViewBLL;
			}
			set
			{
				iHouseInfoViewBLL= value;
			}
		}
		#endregion

		#region 11 业务接口 IOrderInfoDAL
		IOrderInfoBLL iOrderInfoBLL;
		public IOrderInfoBLL IOrderInfoBLL
		{
			get
			{
				if(iOrderInfoBLL==null)
					iOrderInfoBLL= new OrderInfoBLL();
				return iOrderInfoBLL;
			}
			set
			{
				iOrderInfoBLL= value;
			}
		}
		#endregion

		#region 12 业务接口 IOrderInfoViewDAL
		IOrderInfoViewBLL iOrderInfoViewBLL;
		public IOrderInfoViewBLL IOrderInfoViewBLL
		{
			get
			{
				if(iOrderInfoViewBLL==null)
					iOrderInfoViewBLL= new OrderInfoViewBLL();
				return iOrderInfoViewBLL;
			}
			set
			{
				iOrderInfoViewBLL= value;
			}
		}
		#endregion

		#region 13 业务接口 IOrderItemDAL
		IOrderItemBLL iOrderItemBLL;
		public IOrderItemBLL IOrderItemBLL
		{
			get
			{
				if(iOrderItemBLL==null)
					iOrderItemBLL= new OrderItemBLL();
				return iOrderItemBLL;
			}
			set
			{
				iOrderItemBLL= value;
			}
		}
		#endregion

		#region 14 业务接口 IProductCategoryDAL
		IProductCategoryBLL iProductCategoryBLL;
		public IProductCategoryBLL IProductCategoryBLL
		{
			get
			{
				if(iProductCategoryBLL==null)
					iProductCategoryBLL= new ProductCategoryBLL();
				return iProductCategoryBLL;
			}
			set
			{
				iProductCategoryBLL= value;
			}
		}
		#endregion

		#region 15 业务接口 IProductInfoDAL
		IProductInfoBLL iProductInfoBLL;
		public IProductInfoBLL IProductInfoBLL
		{
			get
			{
				if(iProductInfoBLL==null)
					iProductInfoBLL= new ProductInfoBLL();
				return iProductInfoBLL;
			}
			set
			{
				iProductInfoBLL= value;
			}
		}
		#endregion

		#region 16 业务接口 IRegionDAL
		IRegionBLL iRegionBLL;
		public IRegionBLL IRegionBLL
		{
			get
			{
				if(iRegionBLL==null)
					iRegionBLL= new RegionBLL();
				return iRegionBLL;
			}
			set
			{
				iRegionBLL= value;
			}
		}
		#endregion

		#region 17 业务接口 IShopCategoryDAL
		IShopCategoryBLL iShopCategoryBLL;
		public IShopCategoryBLL IShopCategoryBLL
		{
			get
			{
				if(iShopCategoryBLL==null)
					iShopCategoryBLL= new ShopCategoryBLL();
				return iShopCategoryBLL;
			}
			set
			{
				iShopCategoryBLL= value;
			}
		}
		#endregion

		#region 18 业务接口 IShopInfoDAL
		IShopInfoBLL iShopInfoBLL;
		public IShopInfoBLL IShopInfoBLL
		{
			get
			{
				if(iShopInfoBLL==null)
					iShopInfoBLL= new ShopInfoBLL();
				return iShopInfoBLL;
			}
			set
			{
				iShopInfoBLL= value;
			}
		}
		#endregion

		#region 19 业务接口 IShopInfoCertificateDAL
		IShopInfoCertificateBLL iShopInfoCertificateBLL;
		public IShopInfoCertificateBLL IShopInfoCertificateBLL
		{
			get
			{
				if(iShopInfoCertificateBLL==null)
					iShopInfoCertificateBLL= new ShopInfoCertificateBLL();
				return iShopInfoCertificateBLL;
			}
			set
			{
				iShopInfoCertificateBLL= value;
			}
		}
		#endregion

		#region 20 业务接口 IShopInfoCertificateViewDAL
		IShopInfoCertificateViewBLL iShopInfoCertificateViewBLL;
		public IShopInfoCertificateViewBLL IShopInfoCertificateViewBLL
		{
			get
			{
				if(iShopInfoCertificateViewBLL==null)
					iShopInfoCertificateViewBLL= new ShopInfoCertificateViewBLL();
				return iShopInfoCertificateViewBLL;
			}
			set
			{
				iShopInfoCertificateViewBLL= value;
			}
		}
		#endregion

		#region 21 业务接口 ISpotInfoDAL
		ISpotInfoBLL iSpotInfoBLL;
		public ISpotInfoBLL ISpotInfoBLL
		{
			get
			{
				if(iSpotInfoBLL==null)
					iSpotInfoBLL= new SpotInfoBLL();
				return iSpotInfoBLL;
			}
			set
			{
				iSpotInfoBLL= value;
			}
		}
		#endregion

		#region 22 业务接口 ISysLogDAL
		ISysLogBLL iSysLogBLL;
		public ISysLogBLL ISysLogBLL
		{
			get
			{
				if(iSysLogBLL==null)
					iSysLogBLL= new SysLogBLL();
				return iSysLogBLL;
			}
			set
			{
				iSysLogBLL= value;
			}
		}
		#endregion

		#region 23 业务接口 ISystemConfigDAL
		ISystemConfigBLL iSystemConfigBLL;
		public ISystemConfigBLL ISystemConfigBLL
		{
			get
			{
				if(iSystemConfigBLL==null)
					iSystemConfigBLL= new SystemConfigBLL();
				return iSystemConfigBLL;
			}
			set
			{
				iSystemConfigBLL= value;
			}
		}
		#endregion

		#region 24 业务接口 ITenantInfoDAL
		ITenantInfoBLL iTenantInfoBLL;
		public ITenantInfoBLL ITenantInfoBLL
		{
			get
			{
				if(iTenantInfoBLL==null)
					iTenantInfoBLL= new TenantInfoBLL();
				return iTenantInfoBLL;
			}
			set
			{
				iTenantInfoBLL= value;
			}
		}
		#endregion

		#region 25 业务接口 IUserAccountDAL
		IUserAccountBLL iUserAccountBLL;
		public IUserAccountBLL IUserAccountBLL
		{
			get
			{
				if(iUserAccountBLL==null)
					iUserAccountBLL= new UserAccountBLL();
				return iUserAccountBLL;
			}
			set
			{
				iUserAccountBLL= value;
			}
		}
		#endregion

		#region 26 业务接口 IUserFavoriteDAL
		IUserFavoriteBLL iUserFavoriteBLL;
		public IUserFavoriteBLL IUserFavoriteBLL
		{
			get
			{
				if(iUserFavoriteBLL==null)
					iUserFavoriteBLL= new UserFavoriteBLL();
				return iUserFavoriteBLL;
			}
			set
			{
				iUserFavoriteBLL= value;
			}
		}
		#endregion

		#region 27 业务接口 IUserFavoriteViewDAL
		IUserFavoriteViewBLL iUserFavoriteViewBLL;
		public IUserFavoriteViewBLL IUserFavoriteViewBLL
		{
			get
			{
				if(iUserFavoriteViewBLL==null)
					iUserFavoriteViewBLL= new UserFavoriteViewBLL();
				return iUserFavoriteViewBLL;
			}
			set
			{
				iUserFavoriteViewBLL= value;
			}
		}
		#endregion

		#region 28 业务接口 IUserInfoDAL
		IUserInfoBLL iUserInfoBLL;
		public IUserInfoBLL IUserInfoBLL
		{
			get
			{
				if(iUserInfoBLL==null)
					iUserInfoBLL= new UserInfoBLL();
				return iUserInfoBLL;
			}
			set
			{
				iUserInfoBLL= value;
			}
		}
		#endregion

		#region 29 业务接口 IUserInfoCertificateDAL
		IUserInfoCertificateBLL iUserInfoCertificateBLL;
		public IUserInfoCertificateBLL IUserInfoCertificateBLL
		{
			get
			{
				if(iUserInfoCertificateBLL==null)
					iUserInfoCertificateBLL= new UserInfoCertificateBLL();
				return iUserInfoCertificateBLL;
			}
			set
			{
				iUserInfoCertificateBLL= value;
			}
		}
		#endregion

		#region 30 业务接口 IUserInfoCertificateViewDAL
		IUserInfoCertificateViewBLL iUserInfoCertificateViewBLL;
		public IUserInfoCertificateViewBLL IUserInfoCertificateViewBLL
		{
			get
			{
				if(iUserInfoCertificateViewBLL==null)
					iUserInfoCertificateViewBLL= new UserInfoCertificateViewBLL();
				return iUserInfoCertificateViewBLL;
			}
			set
			{
				iUserInfoCertificateViewBLL= value;
			}
		}
		#endregion

		#region 31 业务接口 IUserInfoExtDAL
		IUserInfoExtBLL iUserInfoExtBLL;
		public IUserInfoExtBLL IUserInfoExtBLL
		{
			get
			{
				if(iUserInfoExtBLL==null)
					iUserInfoExtBLL= new UserInfoExtBLL();
				return iUserInfoExtBLL;
			}
			set
			{
				iUserInfoExtBLL= value;
			}
		}
		#endregion

		#region 32 业务接口 IUserInfoExtViewDAL
		IUserInfoExtViewBLL iUserInfoExtViewBLL;
		public IUserInfoExtViewBLL IUserInfoExtViewBLL
		{
			get
			{
				if(iUserInfoExtViewBLL==null)
					iUserInfoExtViewBLL= new UserInfoExtViewBLL();
				return iUserInfoExtViewBLL;
			}
			set
			{
				iUserInfoExtViewBLL= value;
			}
		}
		#endregion

		#region 33 业务接口 IUserInfoViewDAL
		IUserInfoViewBLL iUserInfoViewBLL;
		public IUserInfoViewBLL IUserInfoViewBLL
		{
			get
			{
				if(iUserInfoViewBLL==null)
					iUserInfoViewBLL= new UserInfoViewBLL();
				return iUserInfoViewBLL;
			}
			set
			{
				iUserInfoViewBLL= value;
			}
		}
		#endregion

		#region 34 业务接口 IUserOrderRecordDAL
		IUserOrderRecordBLL iUserOrderRecordBLL;
		public IUserOrderRecordBLL IUserOrderRecordBLL
		{
			get
			{
				if(iUserOrderRecordBLL==null)
					iUserOrderRecordBLL= new UserOrderRecordBLL();
				return iUserOrderRecordBLL;
			}
			set
			{
				iUserOrderRecordBLL= value;
			}
		}
        #endregion
        #region 34 业务接口 IUserBLL
        IUserBLL iUserBLL;
        public IUserBLL IUserBLL
        {
            get
            {
                if (iUserBLL == null)
                    iUserBLL = new UserBLL();
                return iUserBLL;
            }
            set
            {
                iUserBLL = value;
            }
        }
        #endregion
        #region 34 业务接口 iPermissionBLL
        IPermissionBLL iPermissionBLL;
        public IPermissionBLL IPermissionBLL { get {
                if (iPermissionBLL == null)
                    iPermissionBLL = new PermissionBLL();
                return iPermissionBLL;
            } set { iPermissionBLL = value; } }
        #endregion
        #region 34 业务接口 Role
        IRoleBLL iRoleBLL;
        public IRoleBLL IRoleBLL
        {
            get
            {
                if (iRoleBLL == null)
                    iRoleBLL = new RoleBLL();
                return iRoleBLL;
            }
            set { iRoleBLL = value; }
        }
        IShopToDaySetBll iShopToDaySetBll;
        public IShopToDaySetBll IShopToDaySetBll
        {
            get
            {
                if (iShopToDaySetBll == null)
                    iShopToDaySetBll = new ShopToDayPriceSetBLL();
                return iShopToDaySetBll;
            }
            set
            {
                iShopToDaySetBll = value;
            }
        }
        #endregion
        #region 业务接口IFunctionBLL
        IFunctionBLL iFunctionBLL;
        public IFunctionBLL IFunctionBLL
        {
            get
            {
                if (iFunctionBLL == null)
                {
                    iFunctionBLL = new FunctionBLL();
                    
                }
                return iFunctionBLL;
            }
            set
            {
                iFunctionBLL = value;
            }
        }

        #endregion
        #region 业务接口IFuncPermissionBLL
        IFuncPermissionBLL iFuncPermissionBLL;
        public IFuncPermissionBLL IFuncPermissionBLL
        {
            get
            {
                if (iFuncPermissionBLL == null) { 
                        iFuncPermissionBLL = new FuncPermissionBLL();

                }
                return iFuncPermissionBLL;
            }
            set
            {
                iFuncPermissionBLL = value;
            }
        }

        #endregion
        #region 业务接口IUserRoleBLL
        IUserRoleBLL iUserRoleBLL;
        public IUserRoleBLL IUserRoleBLL
        {
            get
            {
                if (iUserRoleBLL == null)
                {
                    iUserRoleBLL = new UserRoleBLL();

                }
                return iUserRoleBLL;
            }
            set
            {
                iUserRoleBLL = value;
            }
        }
        

        #endregion

    }

}