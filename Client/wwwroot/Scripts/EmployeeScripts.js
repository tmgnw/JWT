var Departments = [];
$(document).ready(function () {
    table = $('#Employee').dataTable({
        "ajax": {
            url: "/Employee/LoadEmployee",
            type: "GET",
            dataType: "json"
        },
        "columnDefs": [{
            "targets": [8],
            "orderable": false
        }],
        dom: 'Bfrtip',
        buttons: [
            'csv', 'excel', 'pdf'
        ],
        "columns": [
            { "data": "fullName" },
            { "data": "departmentName" },
            //{ "data": "firstName" },
            //{ "data": "lastName" },
            { "data": "email" },
            {
                "data": "birthDate", "render": function (data) {
                    return moment(data).format('DD MMMM YYYY');
                }
            },
            { "data": "phoneNumber" },
            { "data": "address" },
            {
                "data": "createDate", "render": function (data) {
                    return moment(data).format('DD MMMM YYYY, h:mm a');
                }
            },
            {
                "data": "updateDate", "render": function (data) {
                    var dateupdate = "Not Updated Yet";
                    var nulldate = null;
                    if (data == nulldate) {
                        return dateupdate;
                    } else {
                        return moment(data).format('DD MMMM YYYY, h:mm a');
                    }
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return " <td><button type='button' class='btn btn-warning' id='Update' onclick=GetById('" + row.id + "');>Edit</button> <button type='button' class='btn btn-danger' id='Delete' onclick=Delete('" + row.id + "');>Delete</button ></td >";
                }
            },
        ]
    });

    LoadDepartment($('#DepartmentOption'));
});
//------------------------------------------------------------//
document.getElementById("Add").addEventListener("click", function () {
    $('#Id').val('');
    //$('#Name').val('');
    $('#FirstName').val('');
    $('#LastName').val('');
    $('#Email').val('');
    $('#BirthDate').val('');
    $('#PhoneNumber').val('');
    $('#Address').val('');
    $('#Save').show();
    $('#Update').hide();
});
//------------------------------------------------------------//
function GetById(Id) {
    $.ajax({
        url: "/Employee/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            //const obj = JSON.parse(result);
            $('#Id').val(result[0].id);
            //$('#Name').val(result.name);
            $('#Firstname').val(result[0].firstName);
            $('#Lastname').val(result[0].lastName);
            $('#Email').val(result[0].email);
            $('#Birthdate').val(moment(result[0].birthDate).format('YYYY-MM-DD'));
            $('#Phonenumber').val(result[0].phoneNumber);
            $('#Address').val(result[0].address);
            $('#DepartmentOption').val(result[0].department_Id);

            $('#myModal').modal('show');
            $('#Update').show();
            $('#Save').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText)
        }
    });
}
//------------------------------------------------------------//
function LoadDepartment(element) {
    if (Departments.length === 0) {
        $.ajax({
            type: "Get",
            url: "/Department/LoadDepartment",
            success: function (data) {
                Departments = data.data;
                renderDepartment(element);
            }
        });
    }
    else {
        renderDepartment(element);
    }
}

function renderDepartment(element) {
    var $option = $(element);
    $option.empty();
    $option.append($('<option/>').val('0').text('Select Department').hide());
    $.each(Departments, function (i, val) {
        $option.append($('<option/>').val(val.id).text(val.name));
    });
}
//------------------------------------------------------------//
function ClearScreen() {
    $('#myModal').modal('hide');
    $('#Id').val('');
    $('#Firstname').val('');
    $('#Lastname').val('');
    $('#Email').val('');
    $('#Birthdate').val('');
    $('#Phonenumber').val('');
    $('#Address').val('');
    $('#DepartmentOption').val('');
}
//------------------------------------------------------------//
function Save() {
    $.fn.dataTable.ext.errMode = 'none';
    var table = $('#Employee').DataTable({
        "ajax": {
            url: "/Employee/LoadEmployee"
        }
    });
    var Employee = new Object();
    Employee.FirstName = $('#Firstname').val();
    Employee.LastName = $('#Lastname').val();
    Employee.Email = $('#Email').val();
    Employee.BirthDate = $('#Birthdate').val();
    Employee.PhoneNumber = $('#Phonenumber').val();
    Employee.Address = $('#Address').val();
    Employee.Department_Id = $('#DepartmentOption').val();
        $.ajax({
            type: 'POST',
            url: '/Employee/InsertOrUpdate',
            data: Employee,
        }).then((result) => {
            if (result.statusCode == 201) {
                Swal.fire({
                    icon: 'success',
                    potition: 'center',
                    title: 'Employee Add Successfully',
                    timer: 2000
                }).then(function () {
                    table.ajax.reload();
                    ClearScreen();
                    //$('#myModal').modal('hide');
                    //$('#Id').val('');
                    //$('#Firstname').val('');
                    //$('#Lastname').val('');
                    //$('#Email').val('');
                    //$('#Birthdate').val('');
                    //$('#Phonenumber').val('');
                    //$('#Address').val('');
                    //$('#DepartmentOption').val('');
                });
            }
            else {
                Swal.fire('Error', 'Failed to Add', 'error');
            }
        })
}
//------------------------------------------------------------//
function Edit() {
    $.fn.dataTable.ext.errMode = 'none';
    var table = $('#Employee').DataTable({
        "ajax": {
            url: "/Employee/LoadEmployee"
        }
    });
    var Employee = new Object();
    Employee.Id = $('#Id').val();
    Employee.FirstName = $('#Firstname').val();
    Employee.LastName = $('#Lastname').val();
    Employee.Email = $('#Email').val();
    Employee.BirthDate = $('#Birthdate').val();
    Employee.PhoneNumber = $('#Phonenumber').val();
    Employee.Address = $('#Address').val();
    Employee.Department_Id = $('#DepartmentOption').val();
    $.ajax({
        type: 'POST',
        url: '/Employee/InsertOrUpdate',
        data: Employee
    }).then((result) => {
        if (result.statusCode == 200) {
            Swal.fire({
                icon: 'success',
                potition: 'center',
                title: 'Employee Update Successfully',
                timer: 2500
            }).then(function () {
                table.ajax.reload();
                ClearScreen();
                //$('#myModal').modal('hide');
                //$('#Id').val('');
                //$('#Firstname').val('');
                //$('#Lastname').val('');
                //$('#Email').val('');
                //$('#Birthdate').val('');
                //$('#Phonenumber').val('');
                //$('#Address').val('');
                //$('#DepartmentOption').val('');
            });
        } else {
            Swal.fire('Error', 'Failed to Update', 'error');
        }
    })
}
//------------------------------------------------------------//
function Delete(Id) {
    $.fn.dataTable.ext.errMode = 'none';
    var table = $('#Employee').DataTable({
        "ajax": {
            url: "/Employee/LoadEmployee"
        }
    });
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Employee/Delete/",
                data: { Id: Id }
            }).then((result) => {
                if (result.statusCode == 200) {
                    Swal.fire({
                        icon: 'success',
                        position: 'center',
                        title: 'Delete Successfully',
                        timer: 2000
                    }).then(function () {
                        table.ajax.reload();
                        ClearScreen();
                        //$('#myModal').modal('hide');
                        //$('#Id').val('');
                        //$('#Firstname').val('');
                        //$('#Lastname').val('');
                        //$('#Email').val('');
                        //$('#Birthdate').val('');
                        //$('#Phonenumber').val('');
                        //$('#Address').val('');
                        //$('#DepartmentOption').val('');
                    });
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'error',
                        text: 'Failed to Delete',
                    })
                }
            })
        }
    });
}