var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("admin")) {
        loadDataTable("admin");
    }
    else {
        if (url.includes("customer")) {
            loadDataTable("customer");
        }
        else {
            loadDataTable("all");
        }
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        order: [[0, 'desc']],
        "ajax": { url: "/user/getall?role=" + role },
        "columns": [
            { data: 'Id', "width": "5%" },
            { data: 'email', "width": "25%" },
            { data: 'name', "width": "20%" },
            { data: 'phoneNumber', "width": "20%" },
            { data: 'role', "width": "10%" },
            {
                //data: 'Id',
                //"render": function (data) {
                //    return `<div class="w-75 btn-group" role="group">
                //    <a href="/user/orderDetail?orderId=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i></a>
                //    </div>`
                //},
                //"width": "10%"
            }
        ],
    })
}