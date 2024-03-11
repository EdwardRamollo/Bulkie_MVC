
var DataTable;
$(Document).ready(function () {
    var url = window.location.search; // get complete url.
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        }
        else {
            if (url.includes("pending")) {
                loadDataTable("pending");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    loadDataTable("all");
                }
            }
        }
    }
        
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall?status=' + status },
        // data: data, -- because its an ajax call, data will be called /passed automatically
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "25%" },
            { data: 'phoneNumber', "width": "20%" },
            { data: 'applicationUser.email', "width": "20%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data/* basically the id */) {
                    return `<div class="w-75 btn-group" role="group">
                      <a href="/admin/order/details?orderid=${data}" class="btn btn-primary mx-2> <i class="bi bi-pencil-square"></i></a> &nbsp;
                      
                    </div >`
                },
                "width": "10%"
            },
    ]
  });
}




