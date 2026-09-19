

$(document).ready(function () {
    debugger
    
    loaddata();


});// end of doc

function loaddata(){
    proData = [];
    debugger
    $.ajax({
        url: "/Admin/Product/GetData",
        type: "GET",
        contentType: "application/json",
        async: false,
        success: function (data) {
            $.each(data, function (key, value) {
                editURL = '/Admin/Product/Edit/'+ value.id
                var editbtn = "<a href='" + editURL +"' class='btn btn-primary'>Edit</a>";
                var deletebtn = "<a onclick=DeleteItem(" + value.id + ") class='btn btn-danger'>Delete</a>";
                var hdn = "<input type='hidden' value=" + action + "/>";
                var action = editbtn + "    " + deletebtn + hdn;
                debugger
                proData.push([value.name, value.description.substr(0,30) + '...', value.price, value.category.name, action]);
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
                url: "/Admin/Product/DeleteProduct/"+id,
                type: "Delete",
                success: function (data) {
                    debugger
                    if (data.success) {
                       
                        toaster.success(data.message);
                    }
                    else
                    {
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










//$.each(data, function (key, value) {
//    var editbtn = "<a onclick='FunEdit(this)' class='btn btn-primary'>Edit</a>";
//    var deletebtn = "<a onclick='FunDelete(this)' class='btn btn-danger'>Delete</a>";
//    var hdn = "<input type='hidden' value=" + value.category.name + "/>";
//    var action = editbtn + "    " + deletebtn + hdn;
//    proData.push([value.name, value.description, value.price, value.category.name, action]);
//})

//$(document).ready(function () {
//    //let table = new DataTable('#MyTable')

//    $('#MyTable').DataTable(
//        {
//            ajax: {
//                url: "/Admin/Product/GetData",
//                type: "GET",
//                dataType:"JSON"
//            },
//            processing: true,
//            serverSide: true,
//            filter: true,
//            columns: [
//                { data: "name", name: "name", "className": "my-class" },
//                { data: "price", name: "price", "className": "my-class" },
//                { data: "description", name: "description", "className": "my-class" },
//                //{ data: "category.name", name: "category" }
//            ]
//        }
//    );
