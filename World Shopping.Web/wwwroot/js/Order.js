

$(document).ready(function () {
    //let table = new DataTable('#MyTable')
    loaddata();


});// end of doc

function loaddata() {
    proData = [];
    $.ajax({
        url: "/Admin/Order/GetData",
        type: "GET",
        contentType: "application/json",
        async: false,
        success: function (data) {
            $.each(data, function (key, value) {
                editURL = '/Admin/Order/Details/' + value.id
                var editbtn = "<a href='" + editURL + "' class='btn btn-warning'>Details</a>";
                var hdn = "<input type='hidden' value=" + action + "/>";
                var action = editbtn + "    " + hdn;
                proData.push([value.id, value.name, value.phone, value.applicationUser.email, value.orderStatus, value.totalPrice, action]);
            })
        },
        err: function (err) {
            //err
        }
    });


    $('#MyTable').DataTable({
        data: proData
    });

}

function DeleteItem(id) {

    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        debugger
        if (result.isConfirmed) {
            $.ajax({
                url: "/Admin/Order/Details/" + id,
                type: "Delete",
                success: function (data) {
                    debugger
                    if (data.success) {
                        //proData.ajax.reload;
                        //loaddata();
                        toaster.success(data.message);
                    }
                    else {
                        toaster.error(data.message);
                    }
                }
            });
            Swal.fire({
                title: "Deleted!",
                text: "Your file has been deleted.",
                icon: "success"
            });
        }
    });
}







//========================================================================================

//value.id, value.name, value.phoneNumbe.phoneNumber, value.applicationUser.email, value.orderStatus, value.totalPrice, action


//$(document).ready(function () {
//    //let table = new DataTable('#MyTable')
//    loaddata();


//});// end of doc

//function loaddata() {
//    proData = [];
//    $.ajax({
//        url: "/Admin/Order/GetData",
//        type: "GET",
//        contentType: "application/json",
//        async: false,
//        success: function (data) {
//            $.each(data, function (key, value) {
//                editURL = '/Admin/Order/Edit/' + value.id
//                var editbtn = "<a href='" + editURL + "' class='btn btn-primary'>Edit</a>";
//                var deletebtn = "<a onclick=DeleteItem(" + value.id + ") class='btn btn-danger'>Delete</a>";
//                var hdn = "<input type='hidden' value=" + action + "/>";
//                var action = editbtn + "    " + deletebtn + hdn;
//                proData.push([value.name, value.description, value.price, value.category.name, action]);
//            })
//        },
//        err: function (err) {
//            //err
//        }
//    });


//    $('#MyTable').DataTable({
//        data: proData
//    });

//}

//function DeleteItem(id) {

//    Swal.fire({
//        title: "Are you sure?",
//        text: "You won't be able to revert this!",
//        icon: "warning",
//        showCancelButton: true,
//        confirmButtonColor: "#3085d6",
//        cancelButtonColor: "#d33",
//        confirmButtonText: "Yes, delete it!"
//    }).then((result) => {
//        debugger
//        if (result.isConfirmed) {
//            $.ajax({
//                url: "/Admin/Product/DeleteProduct/" + id,
//                type: "Delete",
//                success: function (data) {
//                    debugger
//                    if (data.success) {
//                        //proData.ajax.reload;
//                        //loaddata();
//                        toaster.success(data.message);
//                    }
//                    else {
//                        toaster.error(data.message);
//                    }
//                }
//            });
//            Swal.fire({
//                title: "Deleted!",
//                text: "Your file has been deleted.",
//                icon: "success"
//            });
//        }
//    });
//}
