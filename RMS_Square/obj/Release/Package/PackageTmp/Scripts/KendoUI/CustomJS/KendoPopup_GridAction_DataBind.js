
var PopupWindow = $("#PopupWindow").kendoWindow({
    //width: "60%",
    //height: "50%",
    draggable: true,
    modal: true,
    resizable: false,
    title: "LOV"
}).data("kendoWindow");




function AutoFitColumn(GridID) {
    var grid = $("#" + GridID).data("kendoGrid");
    for (var i = 0; i < grid.columns.length; i++) {
        grid.autoFitColumn(i);
    }
}


function FilterToGrid(GridID, ValueField) {
    var grid = $("#" + GridID).data("kendoGrid");
    var columns = grid.columns;
    if (columns.length > 0) {
        for (var i = 0; i < columns.length; i++) {
            var col = columns[i];
            $("#" + GridID).data("kendoGrid").dataSource.filter({
                logic: "or",
                filters: [{ field: col.field, operator: "contains", value: ValueField },]
            });
        }
    }
}


function AddRowToGrid(GridID) {
    var addflag = 1; // For Row Add in Detail Grid using enter Key Press

    var grid = $("#" + GridID).data("kendoGrid");

    $("#" + GridID).data().kendoGrid.bind('dataBound', function () {
        this.element.find('tbody tr:first').addClass('k-state-selected');
    });
    var dataSource = $("#" + GridID).data("kendoGrid").dataSource;
    var addData = dataSource.data();
    if (addData != null) { // For Add row when Exiting Data in Grid
        for (var i = 0; i < addData.length; i++) {
            if (addData[i].val == "") {
                addflag = 0;
            }
        }
    }
    if (addflag == 1) {
        grid.addRow();
    }
}


function SetMasterData(data) {
    $.each(data, function (key, value) {
        $('#' + key).val(value);
    });
}

function SetDetailData(GridID, data) {
    $("#" + GridID).data('kendoGrid').dataSource.data(data);
}