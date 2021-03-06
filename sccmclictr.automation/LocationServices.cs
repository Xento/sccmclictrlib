﻿//SCCM Client Center Automation Library (SCCMCliCtr.automation)
//Copyright (c) 2018 by Roger Zander

//This program is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or any later version. 
//This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. 
//GNU General Public License: http://www.gnu.org/licenses/lgpl.html

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace sccmclictr.automation.functions
{
    public class locationservices : baseInit
    {
        internal Runspace remoteRunspace;
        internal TraceSource pSCode;
        internal ccm baseClient;

        public locationservices(Runspace RemoteRunspace, TraceSource PSCode, ccm oClient)
            : base(RemoteRunspace, PSCode)
        {
            remoteRunspace = RemoteRunspace;
            pSCode = PSCode;
            baseClient = oClient;
        }

        public List<BoundaryGroupCache> BoundaryGroupCacheList
        {
            get
            {
                List<BoundaryGroupCache> lCache = new List<BoundaryGroupCache>();
                List<PSObject> oObj = GetObjects(@"ROOT\ccm\LocationServices", "SELECT * FROM BoundaryGroupCache", true);
                foreach (PSObject PSObj in oObj)
                {
                    //Get AppDTs sub Objects
                    BoundaryGroupCache oCIEx = new BoundaryGroupCache(PSObj, remoteRunspace, pSCode);

                    oCIEx.remoteRunspace = remoteRunspace;
                    oCIEx.pSCode = pSCode;
                    lCache.Add(oCIEx);
                }

                return lCache;
            }
        }

        /// <summary>
        /// Source:ROOT\ccm\LocationServices
        /// </summary>
        public class BoundaryGroupCache
        {
            internal baseInit oNewBase;

            //Constructor
            public BoundaryGroupCache(PSObject WMIObject, Runspace RemoteRunspace, TraceSource PSCode)
            {
                remoteRunspace = RemoteRunspace;
                pSCode = PSCode;
                oNewBase = new baseInit(remoteRunspace, pSCode);

                __CLASS = WMIObject.Properties["__CLASS"].Value as string;
                __NAMESPACE = WMIObject.Properties["__NAMESPACE"].Value as string;
                __RELPATH = WMIObject.Properties["__RELPATH"].Value as string;
                __INSTANCE = true;
                this.WMIObject = WMIObject;
                BoundaryGroupIDs = WMIObject.Properties["BoundaryGroupIDs"].Value as String[];
                CacheToken = WMIObject.Properties["CacheToken"].Value as String;
            }

            #region Properties

            internal string __CLASS { get; set; }
            internal string __NAMESPACE { get; set; }
            internal bool __INSTANCE { get; set; }
            internal string __RELPATH { get; set; }
            internal PSObject WMIObject { get; set; }
            internal Runspace remoteRunspace;
            internal TraceSource pSCode;
            public String[] BoundaryGroupIDs { get; set; }
            public String CacheToken { get; set; }
            #endregion

            #region Methods

            /// <summary>
            /// Delete BoundaryGroupCache
            /// </summary>
            /// <returns>true = success</returns>
            public bool Delete()
            {
                bool bResult = false;

                try
                {
                    oNewBase.GetStringFromPS("[wmi]'" + __NAMESPACE + ":" + __RELPATH + "' | remove-wmiobject");
                    bResult = true;
                }
                catch { }

                return bResult;
            }

            #endregion

        }

    }
}
