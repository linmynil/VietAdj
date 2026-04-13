(function () {
    appModule.controller('tenant.views.OldList.modalCreateFinal', [
        '$scope', '$uibModalInstance'
        , 'abp.services.app.invoice'
        , 'abp.services.app.timeSheet',
        function ($scope, $uibModalInstance, invoiceService, timesheetService) {
            var vm = this;
            vm.listTimesheets = [];
            vm.listInvoices = [];
            vm.selectedInvoices = [];
            vm.selectedTimesheets = [];
            vm.OC = true;
            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');

                $('#invoiceDate').daterangepicker({
                    singleDatePicker: true,
                    locale: {
                        format: 'DD-MM-YYYY'
                    }
                });                

            });


            vm.selectTimesheet = function (row) {
                var index = vm.selectedTimesheets.indexOf(row);
                if (index > -1) {
                    vm.selectedTimesheets.splice(index, 1);
                } else {
                    vm.selectedTimesheets.push(row);
                }
                console.log('selectedTimesheets', vm.selectedTimesheets);
                js_CalculateRemain();
                
            }

            var TotalAdvInvoiceAMT = 0;
            vm.selectInvoice = function (row) {
                TotalAdvInvoiceAMT = 0;
                var index = vm.selectedInvoices.indexOf(row);
                if (index > -1) {
                    vm.selectedInvoices.splice(index, 1);
                } else {
                    vm.selectedInvoices.push(row);
                }
                console.log('selectInvoice', vm.selectedInvoices);

                vm.selectedInvoices.forEach(function (item) {
                    console.log(item.advRemainAMT);
                    TotalAdvInvoiceAMT += item.advRemainAMT;
                });
                console.log('total adv', TotalAdvInvoiceAMT);
                //$('#totalAdv').val(TotalAdvInvoiceAMT);
                vm.totalAdv = TotalAdvInvoiceAMT;
                js_CalculateRemain();
            }

            vm.save = function () {
                let invoiceDate = moment(vm.invoiceDate, 'DD-MM-YYYY').format();
                let listTSIds = $.map(vm.selectedTimesheets, function (item) {
                    return item.timeSheetID;
                });
                let listInvoiceIds = $.map(vm.selectedInvoices, function (item) {
                    return item.invoiceID;
                });
                let postdata = {
                    invoiceCode: vm.invoiceCode
                    , invoiceDate: invoiceDate
                    , claimID: vm.claimID
                    , customerID: vm.customerID
                    , nonVAT: vm.nonVAT
                    , listTSIds: listTSIds
                    , listInvoiceIds: listInvoiceIds
                }
                console.log('postdata', postdata);

                invoiceService.createFinalInvoice(postdata).then(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close();
                }).finally(function () {
                    vm.saving = false;
                });
            };
          
            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            vm.getAdvInvoices = function () {
                invoiceService.getAdvanceInvoices(vm.claimID)
                    .then(function (result) {
                        console.log('getadvinvoices data', result.data);
                        vm.listInvoices = result.data;
                    })
            };

            vm.getTimesheets = function () {
                timesheetService.getTimesheetToCreateInvoice(vm.claimID)
                    .then(function (result) {
                        console.log('timesheetService data', result.data);
                        vm.listTimesheets = result.data;
                    })
            };

            var RemainTimeSheetAMT = 0;
            function js_CalculateRemain() {
                var GrandTotal = 0;
                vm.selectedTimesheets.forEach(function (item) {
                    GrandTotal += item.grandAMT;
                });

                console.log('GrandTotal', GrandTotal);
                RemainTimeSheetAMT = (GrandTotal - TotalAdvInvoiceAMT);
                vm.remain = Math.max(RemainTimeSheetAMT, 0);

                if (RemainTimeSheetAMT < 0) {
                }
                else {
                }
            };

            vm.calVAT = function () {
                vm.listTimesheets.forEach(function (item) {
                    item.taxAMT = (item.proFeeGrandAMT + item.expenseAMT) / 10;
                    if (document.getElementById("nonVAT").checked) {
                        item.taxAMT = 0;
                    }                    
                    item.grandAMT = item.proFeeGrandAMT + item.expenseAMT + item.taxAMT;
                });
            };

            vm.invoiceNo = function () {
                if (vm.invoiceCode != "") {
                    $("#invoiceCode").removeClass("has-error");
                    vm.OC = false;
                } else {
                    $("#invoiceCode").addClass("has-error");
                    vm.OC = true;
                }
            };
            function init() {
                let claimID = $scope.$parent.vm.claimID;
                vm.claimID = claimID;
                vm.customerID = $scope.$parent.vm.customerID;
                vm.customerName = $scope.$parent.vm.customerName;
                console.log('claimID');
                vm.getAdvInvoices();
                vm.getTimesheets();
                vm.invoiceNo();
            }

            init();
        }
    ]);
})();