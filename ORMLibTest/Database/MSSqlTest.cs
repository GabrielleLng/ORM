﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using ORMLib.Generics;
using ORMLib.Constants;
using ORMLibTest.Modele;
using ORMLib.exception;
using ORMLib.Database;

namespace ORMLibTest
{
    [TestClass]
    public class MSSqlTest
    {
        private MSSql database = new MSSql("localhost", "1433", "devdb", "sa", "P@55w0rd");

        [TestMethod]
        public void TestExecuteWithSelectStatementAndWithParameters()
        {
            //GIVEN
            string selectReqWithParameters = @"SELECT * FROM contacts WHERE email = @email";
            List<DbParameter> selectParameters = new List<DbParameter>();
            selectParameters.Add(new SqlParameter("@email", "canserkan.uren@gmail.com"));

            //WHEN
            List<Contacts> contactsSelectWithParameter = database.Execute<Contacts>(selectReqWithParameters, selectParameters);

            //THEN
            Contacts contact = contactsSelectWithParameter.First();

            Assert.AreEqual("canserkan.uren@gmail.com", contact.email);
            Assert.IsNotNull(contactsSelectWithParameter);
            Assert.IsNotNull(contact);
        }

        [TestMethod]
        public void TestExecuteWithInsertStatementAndWithParameters()
        {
            //GIVEN
            string insertReq = @"INSERT INTO contacts (name, address, email) VALUES (@name, @address, @email)";
            List<DbParameter> insertParameters = new List<DbParameter>();
            insertParameters.Add(new SqlParameter("@name", "TEST CASE"));
            insertParameters.Add(new SqlParameter("@address", "RUE DU TEST"));
            insertParameters.Add(new SqlParameter("@email", "TestEmail@gmail.com"));

            //WHEN
            List<Contacts> contactsInsertWithParameter = database.Execute<Contacts>(insertReq, insertParameters);

            //THEN            
            Assert.IsTrue((contactsInsertWithParameter.Count == 0));
        }

        [TestMethod]
        public void TestExecuteWithUpdateStatementAndWithParameters()
        {
            //GIVEN
            string updateReq = @"UPDATE contacts SET name = @name, address = @address, @email = email WHERE email = @oldEmail";
            List<DbParameter> updateParameters = new List<DbParameter>();
            updateParameters.Add(new SqlParameter("@name", "NAME CHANGED FOR TEST"));
            updateParameters.Add(new SqlParameter("@address", "ADDRESS CHANGED FOR TEST"));
            updateParameters.Add(new SqlParameter("@email", "emailChangedForTest@gmail.com"));
            updateParameters.Add(new SqlParameter("@oldEmail", "TestEmail@gmail.com"));

            //WHEN
            List<Contacts> contactsUpdate = database.Execute<Contacts>(updateReq, updateParameters);

            //THEN
            Assert.IsTrue((contactsUpdate.Count == 0));
        }

        [TestMethod]
        public void TestExecuteWithDeleteStatementAndWithParameters()
        {
            //GIVEN
            string deleteReq = @"DELETE FROM contacts WHERE address = @address";
            List<DbParameter> deleteParameters = new List<DbParameter>();
            deleteParameters.Add(new SqlParameter("@address", "ADDRESS CHANGED FOR TEST"));

            //WHEN
            List<Contacts> contactsDelete = database.Execute<Contacts>(deleteReq, deleteParameters);

            //THEN
            Assert.IsTrue((contactsDelete.Count == 0));
        }
        
        [TestMethod]
        [ExpectedException(typeof(DatabaseException), "An error has occured. Reason :")]
        public void TestExecuteWithWrongSqlStatement()
        {
            //GIVEN
            string deleteReq = @"DELETEZZ FROM contacts WHERE address = @address";
            List<DbParameter> deleteParameters = new List<DbParameter>();
            deleteParameters.Add(new SqlParameter("@address", "ADDRESS CHANGED FOR TEST"));

            //WHEN
            List<Contacts> contactsDelete = database.Execute<Contacts>(deleteReq, deleteParameters);

            //THEN
        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseException), "An error has occured. Reason :")]
        public void TestExecuteWithWrongSelectStatement()
        {
            //GIVEN
            string deleteReq = @"SELECTZZ * FROM contacts WHERE address = @address";
            List<DbParameter> deleteParameters = new List<DbParameter>();
            deleteParameters.Add(new SqlParameter("@address", "ADDRESS CHANGED FOR TEST"));

            //WHEN
            List<Contacts> contactsDelete = database.Execute<Contacts>(deleteReq, deleteParameters);

            //THEN
        }

        [TestMethod]
        [ExpectedException(typeof(WrongSqlRequestException), "Your SQL statement is not a SELECT, INSERT, UPDATE or DELETE statement")]
        public void TestExecuteWithNoSqlStatement()
        {
            //GIVEN
            string deleteReq = String.Empty;

            //WHEN
            List<Contacts> contactsDelete = database.Execute<Contacts>(deleteReq, new List<DbParameter>());

            //THEN
        }
    }
}