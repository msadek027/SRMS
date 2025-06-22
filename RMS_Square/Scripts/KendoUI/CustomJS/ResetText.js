
// Reset Data
function ResetData() {

    $('input[type="hidden"]').val("");
    $('input[type="text"]').val("");
    $('input[type="checkbox"]:checked').prop('checked', true);

    $("textarea").val("");  
    $("select").prop('selectedIndex', 0);

    $(":checkbox").prop('checked', false);
    $('input[type="time"]').val('');

    $(".date").val('');
    $(".SetFocus").focus();
}
function ResetData1() {

    $('input[type="hidden"]').val("");
    $('input[type="text"]').val("");
    $('input[type="checkbox"]:checked').prop('checked', true);
    // $('input[name="rdoButtonName"]').prop('checked', false); 
    $("textarea").val("");
    //$("select").each(function () { this.selectedIndex = 0 });
    $(".SetFocus").focus();

}
function ResetData2() {

    $('input[type="hidden"]').val("");
    $('input[type="text"]').val("");  
    $("textarea").val("");
    $("#cmbSelect option:eq(0)").attr("selected", "selected");

    $(":text").val("");
    $(":textarea").val("");
    $(".hiddenField").val("");

    $(".SetFocus").focus();
}
function RemoveTxtChk() {
    $('.chk').remove();
    $('.txt').remove();

}
//For removing operational & required message after triggering other event
function ClearBorderRequiredMsg() {
    $(".RequiredField").css("border", "1px Solid black"); //Clear Red Color     
    $("#MessageText").html(""); //Clear operation Message  
}


