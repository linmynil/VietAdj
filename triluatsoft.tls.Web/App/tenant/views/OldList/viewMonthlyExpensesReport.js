(function () {
    appModule.controller('tenant.views.OldList.MonthlyExpensesReport', [
        '$scope',
        '$uibModal',
        'abp.services.app.employee',
        function (
            $scope,
            $uibModal,
            employeeService
        ) {
            var vm = this;
            vm.page = 1;
            let fromDate = '';
            let toDate = '';    

            vm.dateRangeOptions = app.createDateRangePickerOptions();
            vm.dateRangeModel = {};

            vm.getEmployees = function () {
                console.log('getemployees');
                employeeService.getEmployeesForClaim({})
                    .then(function (result) {
                        vm.listEmployees = result.data;
                    });
            };
            vm.init = function () {
                vm.getEmployees();
            }
            vm.init();            

            vm.printReport = function (event, printType) {                
                fromDate = vm.dateRangeModel.startDate.format('MM-DD-YYYY');
                toDate = vm.dateRangeModel.endDate.format('MM-DD-YYYY');
                event.preventDefault();
                if (printType == 'inline') {
                    App.startPageLoading({ animate: true });
                }
                //var params = '';
                //load the report by iframe
                if (vm.empId == undefined) {
                    $("#ReportFrame").attr("src", 'report/MonthlyExpenseReport?empID='+ 0 + "&txtFromDate1=" + fromDate + "&txtToDate1=" + toDate + "&lang=" + vm.lang + "&ReportName=Testing&SaveName=Testing Report&ReportType=" + printType + "&ReportPath=Demo");
                    $('#ReportFrame').load(function () {
                        console.log('iframe loaded');
                        App.stopPageLoading({ animate: true });
                    });                
                }
                else {
                $("#ReportFrame").attr("src", 'report/MonthlyExpenseReport?empID=' + vm.empId + "&txtFromDate1=" + fromDate + "&txtToDate1=" + toDate + "&lang=" + vm.lang + "&ReportName=Testing&SaveName=Testing Report&ReportType=" + printType + "&ReportPath=Demo");
                $('#ReportFrame').load(function () {
                    console.log('iframe loaded');   
                    App.stopPageLoading({ animate: true });
                    });
                }
            }
        }
    ]);
})();