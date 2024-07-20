
var DataTable;
$(Document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tableData').DataTable({
        "ajax": { url:'/admin/user/getall'},
        // data: data, -- because its an ajax call, data will be called /passed automatically
        "columns": [
            { data: 'name', "width": "15%" },
            { data: 'email', "width": "15%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'company.name', "width": "15%" },
            { data: '', "width": "15%" },
            {
                data: 'id',
                "render": function (data/* basically the id */) {
                    return `<div class="w-75 btn-group" role="group">
                      <a href="/admin/company/upsert?id=${data}" class="btn btn-primary mx-2> <i class="bi bi-pencil-square"></i> Edit</a> &nbsp;
                      </div >`
                },
                "width": "25%"
            },
    ]
  });
}




