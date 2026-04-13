//****************************************************
//hvtam-08042015-comment: Message definition
//***************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClaimWebsite.Code
{
    public enum MessageKeys
    {
        //Common Errors
        ACCESS_DENIED_ERROR,
        PARAMETERS_ERROR,
        PROCESSING_ERROR,
        UNEXPECTED_ERROR,
        GET_LOST_ERROR,

        //Security messages
        NOT_ENOUGH_PRIVILEGE_ERROR,
        UNABLE_DELETE_GROUP,

        //Claim messages
        REMOVE_ACCOUNT_MANAGER_FAILED,
        REMOVE_USER_FAILED,
        CLAIM_EXISTED,
        UPDATE_NOTHING,
        TOTAL_RESULT_MYCLAIM,
        //Task messages
        UNABLE_DELETE_TASK,
        START_DATE_GREATER_END_DATE,

        //Timesheet messages
        UNABLE_ISSUE_TIMESHEET,
        ISSUED_TIMESHEET_SUCCESSFULLY,
   

        //Cash messages

        //Common messages
        SAVE_FAILED,
        SAVED_SUCCESSFULLY_TMPL,
        DELETED_SUCCESSFULLY_TMPL,
        REQUIRED_FIELD_ERROR_TMPL,

        //Notification message templates
        NO_UPDATE_NFT_TMPL,
        ILA_NO_UPDATE_NFT_TMPL,
        PREP_INFO_NFT_TMPL,
        PREP_WARNING_NFT,

        //Account messages
        CONFIRM_PASS_NOT_MATCHED,
        DEFAULT_PASS_RESERVED,
        PASSWORD_CHANGED_SUCCESSFULLY,
        USER_NAMING_ERROR,
        USER_NAME_EXISTED,
        USER_NAME_VALID,
        STATUS_EXISTED,

        //Report messages
        SELECT_FROM_DATE,
        FROM_DATE_GREATER_TO_DATE,

      // Invoice 
      INVOICE_CREATE_SUCCESSFULLY,
      INVOICE_UPDATE_SUCCESSFULLY,
      INVOICE_APPROVE_SUCCESSFULLY,
      // Customer Officer 
      OFFICER_CREATE_SUCCESSFULLY,
      OFFICER_UPDATE_SUCCESSFULLY,
      OFFICER_APPROVE_SUCCESSFULLY,
      UNISSUED_TIMESHEET_SUCCESSFULLY,
      UNSUBMITED_TIMESHEET_SUCCESSFULLY,
      SUBMITED_TIMESHEET_SUCCESSFULLY,
      UNISSUED_TIMESHEET_NOTFOUND,
      UNISSUED_TIMESHEET_HASINVOICE,
      
      DEBITNOTE_DELETE_HAS_INVOICE,
      DEBITNOTE_DELETE_NOTFOUND,
      DEBITNOTE_DELETE_SUCCESSFULLY,
      DEBITNOTE_UPDATE_SUCCESSFULLY,

       //hvtam-08042015 - SQL exception handling 
      //SQL_UNIQUE_KEY_CONSTRAINT,

    }
}