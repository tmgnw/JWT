var Departments = [];
$(document).ready(function () {
    //load Chart
    Donut();
    Bar();
    //load Datatable
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
                    return " <td><button type='button' class='btn btn-warning' id='Update' onclick=GetById('" + row.email + "');>Edit</button> <button type='button' class='btn btn-danger' id='Delete' onclick=Delete('" + row.email + "');>Delete</button ></td >";
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
function GetById(Email) {
    //debugger;
    $.ajax({
        url: "/Employee/GetById/" + Email,
        //type: "GET",
        //contentType: "application/json;charset=utf-8",
        //dataType: "json",
        //async: false,
        data: { "Email": Email }
    }).then((result) => {
        if (result) {
            $('#Email').val(result.email);
            $('#Firstname').val(result.firstName);
            $('#Lastname').val(result.lastName);
            $('#Birthdate').val(moment(result.birthDate).format('YYYY-MM-DD'));
            $('#Phonenumber').val(result.phoneNumber);
            $('#Address').val(result.address);
            $('#DepartmentOption').val(result.department_Id);

            $('#myModal').modal('show');
            $('#Update').show();
            $('#Save').hide();
        }
    })
        //success: function (result) {
        //    debugger;
            //const obj = JSON.parse(result);
            //$('#Id').val(result[0].id);
            //$('#Name').val(result.name);
        //error: function (errormessage) {
        //    alert(errormessage.responseText)
        //}
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
    Employee.Password = $('#Password').val();
    Employee.BirthDate = $('#Birthdate').val();
    Employee.PhoneNumber = $('#Phonenumber').val();
    Employee.Address = $('#Address').val();
    Employee.Department_Id = $('#DepartmentOption').val();
        $.ajax({
            type: 'POST',
            url: '/Employee/Insert',
            data: Employee,
        }).then((result) => {
            if (result.statusCode === 200 || result.statusCode === 201) {
                Swal.fire({
                    icon: 'success',
                    potition: 'center',
                    title: 'Employee Add Successfully',
                    timer: 2000
                }).then(function () {
                    table.ajax.reload();
                    ClearScreen();
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
    //Employee.Id = $('#Id').val();
    Employee.Email = $('#Email').val();
    Employee.FirstName = $('#Firstname').val();
    Employee.LastName = $('#Lastname').val();
    Employee.BirthDate = $('#Birthdate').val();
    Employee.PhoneNumber = $('#Phonenumber').val();
    Employee.Address = $('#Address').val();
    Employee.Department_Id = $('#DepartmentOption').val();
    $.ajax({
        type: 'POST',
        url: '/Employee/Updatee',
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
            });
        } else {
            Swal.fire('Error', 'Failed to Update', 'error');
        }
    })
}
//------------------------------------------------------------//
function Delete(email) {
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
                data: { email: email }
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
//------------------------------------------------------------//
function Donut() {
    //debugger;
    $.ajax({
        type: 'GET',
        url: '/Employee/GetChart/',
        success: function (data) {
            //debugger;
            Morris.Donut({
                element: 'DonutChart',
                data: $.each(JSON.parse(data), function (index, val) {
                    //debugger;
                    [{
                        label: val.label,
                        value: val.value
                    }]
                }),
                resize: true,
                colors: ['#009efb', '#55ce63', '#2f3d4a']
            });
        }
    })
};
//------------------------------------------------------------//
function Bar() {
    $.ajax({
        type: 'GET',
        url: '/Employee/GetChart/',
        success: function (data) {
            Morris.Bar({
                element: 'BarChart',
                data: $.each(JSON.parse(data), function (index, val) {
                    //debugger;
                    [{
                        label: val.label,
                        value: val.value
                    }]
                }),
                xkey: 'label',
                ykeys: ['value'],
                labels: ['label'],
                barColors: ['#009efb', '#55ce63', '#2f3d4a'],
                hideHover: 'auto',
                gridLineColor: '#eef0f2',
                resize: true
            });
        }
    })
};