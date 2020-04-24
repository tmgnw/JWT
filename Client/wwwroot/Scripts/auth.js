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
    LoadEmp();
    //LoadEmp2();
    $('#hidenav').hide();
});
//------------------------------------------------------------------------------//
function Edit() {
    var table = $('#Employee').DataTable({
        "ajax": {
            url: "/Account/LoadEmployee/"
        }
    });
    var Employee = new Object();
    Employee.email = $('#Email').val();
    Employee.firstName = $('#FirstName').val();
    Employee.lastName = $('#LastName').val();
    Employee.department_Id = $('#DepartmentOption').val();
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
function LoadEmp() {
    debugger;
    $.ajax({
        url: "/Account/LoadEmployee/",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            debugger;
            var object = JSON.parse(result);
            var object2 = JSON.parse(object);
            $('#Email').val(object2.email);
            $('#FirstName').val(object2.firstName);
            $('#LastName').val(object2.lastName);
            $('#BirthDate').val(moment(object2.birthDate).format('YYYY-MM-DD'));
            $('#PhoneNumber').val(object2.phoneNumber);
            $('#Address').val(object2.address);
            $('#DepartmentOption').val(object2.department_Id);
            option = object2.Department_Id;
            optionName = object2.departmentName;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//------------------------------------------------------------------------------//
//function LoadEmp2() {
//    $.ajax({
//        url: "/Account/LoadEmployee/"
//    }).then((result) => {
//        if (result) {
//            $('#Email').val(result.email);
//            $('#Firstname').val(result.firstName);
//            $('#Lastname').val(result.lastName);
//            $('#Birthdate').val(moment(result.birthDate).format('YYYY-MM-DD'));
//            $('#Phonenumber').val(result.phoneNumber);
//            $('#Address').val(result.address);
//            $('#DepartmentOption').val(result.department_Id);
//            option = object2.Department_Id;
//            optionName = object2.departmentName;
//        }
//    })
//}//------------------------------------------------------------------------------//
