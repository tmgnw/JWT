var Departments = [];
var element = $('#DepartmentOption');
var option = 0;
var optionName = "";
$(document).ready(function () {
    $.ajax({
        url: "/Department/LoadDepartment",
        type: "GET",
        success: function (data) {
            //debugger;
            Departments = data.data;
            var $option = $(element);
            $option.empty();
            $option.append($('<option/>').val(option).text(optionName).hide());
            $.each(Departments, function (i, val) {
                //debugger;
                $option.append($('<option/>').val(val.id).text(val.name));
            });
        }
    });
    //debugger;
    $.ajax({
        url: "/Account/LoadEmployee/",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            //debugger;
            $('#Email').val(result.email);
            $('#FirstName').val(result.firstName);
            $('#LastName').val(result.lastName);
            $('#BirthDate').val(moment(result.birthDate).format('YYYY-MM-DD'));
            $('#PhoneNumber').val(result.phoneNumber);
            $('#Address').val(result.address);
            $('#DepartmentOption').val(result.department_Id);
            option = result.department_Id;
            optionName = result.departmentName;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    $('#hidenav').hide();
});
//------------------------------------------------------------------------------//
function Edittt() {
    var table = $('#Employee').DataTable({
        "ajax": {
            url: "/Account/LoadEmployee/"
        }
    });
    var Employee = new Object();
    Employee.email = $('#Email').val();
    //Employee.Password = $('#Password').val();
    Employee.firstName = $('#FirstName').val();
    Employee.lastName = $('#LastName').val();
    Employee.deptModelId = $('#DepartmentOption').val();
    Employee.birthDate = $('#BirthDate').val();
    Employee.phoneNumber = $('#PhoneNumber').val();
    Employee.address = $('#Address').val();
    $.ajax({
        type: 'POST',
        url: '/Account/EditEmp',
        data: Employee
    }).then((result) => {
        //debugger;
        if (result.statusCode === 200 || result.statusCode === 201 || result.statusCode === 204) {
            Swal.fire({
                icon: 'success',
                potition: 'center',
                title: 'Profile Update Successfully',
                timer: 2500
            }).then(function () {
                clearscreen();
            });
        } else {
            Swal.fire('Error', 'Failed to Edit', 'error');
        }
    })
}
//------------------------------------------------------------------------------//