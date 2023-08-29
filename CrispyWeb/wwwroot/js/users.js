var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/User/GetAll' },
        "columns": [
            { data: 'name', "width": "10%" },
            { data: 'email', "width": "15%" },
            { data: 'phoneNumber', "width": "20%" },
            { data: 'company.name', "width": "20%" },
            { data: 'role', "width": "10%" },
            {
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        return `
                        <div class="text-center">
                        <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                        <i class="bi bi-unlock-fill"></i> UnLock
                        </a>
                         <a href="/admin/user/RoleManagement?Id=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:150px;">
                        <i class="bi bi-pencil-square"></i> Permission
                        </a>
                        </div>
                        `
                    }
                    else {
                        return `
                        <div class="text-center">
                        <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                        <i class="bi bi-lock-fill"></i> Lock
                        </a>
                         <a href="/admin/user/RoleManagement?Id=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:150px;">
                        <i class="bi bi-pencil-square"></i> Permission
                        </a>
                        </div>
                        `
                    }
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
} function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    })
}