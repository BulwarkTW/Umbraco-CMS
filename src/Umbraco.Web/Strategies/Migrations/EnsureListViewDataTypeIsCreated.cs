﻿using System;
using System.Collections.Generic;
using System.Linq;
using NPoco;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Rdbms;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Persistence.UnitOfWork;
using Umbraco.Core.Configuration;

namespace Umbraco.Web.Strategies.Migrations
{
    /// <summary>
    /// Creates the built in list view data types
    /// </summary>
    public class EnsureDefaultListViewDataTypesCreated : MigrationStartupHandler
    {
        protected override void AfterMigration(MigrationRunner sender, MigrationEventArgs e)
        {
            if (e.ProductName != GlobalSettings.UmbracoMigrationName) return;

            var target720 = new Version(7, 2, 0);

            if (e.ConfiguredVersion <= target720)
            {
                EnsureListViewDataTypeCreated(e);

            }
        }

        private void EnsureListViewDataTypeCreated(MigrationEventArgs e)
        {
            var syntax = e.MigrationContext.Database.SqlSyntax;

            using (var transaction = e.MigrationContext.Database.GetTransaction())
            {
                try
                {
                    //Turn on identity insert if db provider is not mysql
                    if (syntax.SupportsIdentityInsert())
                        e.MigrationContext.Database.Execute(new Sql(string.Format("SET IDENTITY_INSERT {0} ON ", syntax.GetQuotedTableName("umbracoNode"))));

                    if (e.MigrationContext.Database.Exists<NodeDto>(Constants.DataTypes.DefaultContentListView))
                    {
                        //If this already exists then just exit
                        return;
                    }

                    e.MigrationContext.Database.Insert("umbracoNode", "id", false, new NodeDto { NodeId = Constants.DataTypes.DefaultContentListView, Trashed = false, ParentId = -1, UserId = 0, Level = 1, Path = "-1,-95", SortOrder = 2, UniqueId = new Guid("C0808DD3-8133-4E4B-8CE8-E2BEA84A96A4"), Text = Constants.Conventions.DataTypes.ListViewPrefix + "Content", NodeObjectType = new Guid(Constants.ObjectTypes.DataType), CreateDate = DateTime.Now });
                    e.MigrationContext.Database.Insert("umbracoNode", "id", false, new NodeDto { NodeId = Constants.DataTypes.DefaultMediaListView, Trashed = false, ParentId = -1, UserId = 0, Level = 1, Path = "-1,-96", SortOrder = 2, UniqueId = new Guid("3A0156C4-3B8C-4803-BDC1-6871FAA83FFF"), Text = Constants.Conventions.DataTypes.ListViewPrefix + "Media", NodeObjectType = new Guid(Constants.ObjectTypes.DataType), CreateDate = DateTime.Now });
                    e.MigrationContext.Database.Insert("umbracoNode", "id", false, new NodeDto { NodeId = Constants.DataTypes.DefaultMembersListView, Trashed = false, ParentId = -1, UserId = 0, Level = 1, Path = "-1,-97", SortOrder = 2, UniqueId = new Guid("AA2C52A0-CE87-4E65-A47C-7DF09358585D"), Text = Constants.Conventions.DataTypes.ListViewPrefix + "Members", NodeObjectType = new Guid(Constants.ObjectTypes.DataType), CreateDate = DateTime.Now });
                }
                finally
                {
                    //Turn off identity insert if db provider is not mysql
                    if (syntax.SupportsIdentityInsert())
                        e.MigrationContext.Database.Execute(new Sql(string.Format("SET IDENTITY_INSERT {0} OFF;", syntax.GetQuotedTableName("umbracoNode"))));
                }


                try
                {
                    //Turn on identity insert if db provider is not mysql
                    if (syntax.SupportsIdentityInsert())
                        e.MigrationContext.Database.Execute(new Sql(string.Format("SET IDENTITY_INSERT {0} ON ", syntax.GetQuotedTableName("cmsDataType"))));

                    e.MigrationContext.Database.Insert("cmsDataType", "pk", false, new DataTypeDto { PrimaryKey = -26, DataTypeId = Constants.DataTypes.DefaultContentListView, PropertyEditorAlias = Constants.PropertyEditors.ListViewAlias, DbType = "Nvarchar" });
                    e.MigrationContext.Database.Insert("cmsDataType", "pk", false, new DataTypeDto { PrimaryKey = -27, DataTypeId = Constants.DataTypes.DefaultMediaListView, PropertyEditorAlias = Constants.PropertyEditors.ListViewAlias, DbType = "Nvarchar" });
                    e.MigrationContext.Database.Insert("cmsDataType", "pk", false, new DataTypeDto { PrimaryKey = -28, DataTypeId = Constants.DataTypes.DefaultMembersListView, PropertyEditorAlias = Constants.PropertyEditors.ListViewAlias, DbType = "Nvarchar" });
                }
                finally
                {
                    //Turn off identity insert if db provider is not mysql
                    if (syntax.SupportsIdentityInsert())
                        e.MigrationContext.Database.Execute(new Sql(string.Format("SET IDENTITY_INSERT {0} OFF;", syntax.GetQuotedTableName("cmsDataType"))));
                }



                try
                {
                    //Turn on identity insert if db provider is not mysql
                    if (syntax.SupportsIdentityInsert())
                        e.MigrationContext.Database.Execute(new Sql(string.Format("SET IDENTITY_INSERT {0} ON ", syntax.GetQuotedTableName("cmsDataTypePreValues"))));

                    //defaults for the member list
                    e.MigrationContext.Database.Insert("cmsDataTypePreValues", "id", false, new DataTypePreValueDto { Id = -1, Alias = "pageSize", SortOrder = 1, DataTypeNodeId = Constants.DataTypes.DefaultMembersListView, Value = "10" });
                    e.MigrationContext.Database.Insert("cmsDataTypePreValues", "id", false, new DataTypePreValueDto { Id = -2, Alias = "orderBy", SortOrder = 2, DataTypeNodeId = Constants.DataTypes.DefaultMembersListView, Value = "Name" });
                    e.MigrationContext.Database.Insert("cmsDataTypePreValues", "id", false, new DataTypePreValueDto { Id = -3, Alias = "orderDirection", SortOrder = 3, DataTypeNodeId = Constants.DataTypes.DefaultMembersListView, Value = "asc" });
                    e.MigrationContext.Database.Insert("cmsDataTypePreValues", "id", false, new DataTypePreValueDto { Id = -4, Alias = "includeProperties", SortOrder = 4, DataTypeNodeId = Constants.DataTypes.DefaultMembersListView, Value = "[{\"alias\":\"email\",\"isSystem\":1},{\"alias\":\"username\",\"isSystem\":1},{\"alias\":\"updateDate\",\"header\":\"Last edited\",\"isSystem\":1}]" });
                }
                finally
                {
                    //Turn off identity insert if db provider is not mysql
                    if (syntax.SupportsIdentityInsert())
                        e.MigrationContext.Database.Execute(new Sql(string.Format("SET IDENTITY_INSERT {0} OFF;", syntax.GetQuotedTableName("cmsDataTypePreValues"))));
                }



                transaction.Complete();
            }
        }

    }

}