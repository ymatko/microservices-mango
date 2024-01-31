var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').dataTable({
        "ajax": { url: "/order/getall" },
        "columns": [
            { Data: 'orderheaderid', "width": "5%"}
            { Data: 'email', "width": "25%"}
        ]
    })
}