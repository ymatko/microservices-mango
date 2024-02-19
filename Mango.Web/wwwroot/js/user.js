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

function loadDataTable(role) {
    dataTable = $('#tblData').DataTable({
        order: [[0, 'desc']],
        "ajax": { url: `/user/getall?role=${role}` },
        "columns": [
            { data: 'userId', "width": "30%" },
            { data: 'email', "width": "25%" },
            { data: 'name', "width": "20%" },
            { data: 'phoneNumber', "width": "20%" },
            { data: 'role', "width": "10%" },
            {
                data: 'userId',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/user/userEdit?userId=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i></a>
                    </div>`
                },
                "width": "10%"
            }
        ],
    })
}