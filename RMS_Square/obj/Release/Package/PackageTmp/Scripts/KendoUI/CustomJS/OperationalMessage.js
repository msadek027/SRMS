function ValidationMsg() {  
    toastr.warning("Enter your value according to the marking field!");
}
function AcknowledgeMsg() { 
    toastr.info("Data not found!");
}
function AcknowledgeGrantedMsg() {
    toastr.info("Your request granted!");
}
function OperationMsg(Mode) {
    var msg = "";
    if (Mode == "I")
    {
        msg = "Saved Successfully!";       
    }
   else if (Mode == "U")
   {
       msg = "Updated Successfully!";      
    }
  else if (Mode == "No")
  {
      msg = "Not Saved!"; 
   }
  else if (Mode == "D") {
      msg = "Deleted Successfully!";    
  }
  else if (Mode == "NoDel") {
      msg = "Not Deleted!";  
  }
    toastr.success(msg);

}
function WarningMsg() {
   
    toastr.warning("Duplicate Data!");

}
function ErrorFrmServerMsg(msgValue) {   
   
    toastr.warning(msgValue);
}
function ErrorFrmClientMsg() {
  
    toastr.warning("Error occured!");
}
function CompletedMsg() { 
    toastr.success("Process Completed!");
}


function ExceptionMsg(data, ErrorCode)
{                                   
        var title = data.substring(data.indexOf("<title>") + 7, data.indexOf("</title>"));
        //var body = data.substring(data.indexOf("<body>") + 6, data.indexOf("</body>"));
    
        var ErrorMsg = "Error: " + ErrorCode + ": " + title;  
        toastr.warning(ErrorMsg);
}