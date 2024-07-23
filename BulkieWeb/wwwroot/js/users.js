
var DataTable;
$(Document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url:'/admin/users/getall' },
        // data: data, -- because its an ajax call, data will be called /passed automatically
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "company.name", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data/* basically the id */) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime(); //Convert lockout into an integer, therefore we can make a comparison

                    if (lockout > today) {
                        return `
                        <div class="text-center">
                            <a onClick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor: pointer; width:100px;">
                                <i class="bi bi-unlock-fill"></i> Lock
                            </a>
                            <a href="/Admin/Users/RoleManagement?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:150px;"><!--Pass user id to RoleManagement function-->
                                <i class="bi  bi-pencil-square"></i> Permission
                            </a>
                        </div>
                        `
                    } else {
                        return `
                        <div class="text-center">
                            <a onClick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                <i class="bi bi-lock-fill"></i> Unlock
                            </a>
                            <a href="/Admin/Users/RoleManagement?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:150px;"><!--Pass user id to RoleManagement function-->
                                <i class="bi bi-pencil-square"></i> Permission
                            </a>
                        </div>
                        `
                    }
                   
                },
                "width": "25%"
            }
        ]
    });
}

function LockUnlock(id)
{
    $.ajax({
        type: "POST",
        url: '/Admin/Users/LockUnlock',
        data: JSON.stringify(id),
        contentType:"application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    });
}





