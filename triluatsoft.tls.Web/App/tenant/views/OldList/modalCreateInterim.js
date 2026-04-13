(function () {
    appModule.controller('tenant.views.OldList.modalCreateInterim', [
        '$scope', '$uibModalInstance', 'abp.services.app.invoice',
        function ($scope, $uibModalInstance, invoiceService) {
            var vm = this;

            vm.saving = false;
            vm.item = {};

            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');

                $('#invoiceDate').daterangepicker({
                    singleDatePicker: true,
                    locale: {
                        format: 'DD-MM-YYYY'
                    }
                });


            });

            vm.calc = function js_CalcRevenue() {
                //let NetFee = parseFloat($('#fee').val());
                let NetFee = vm.fee;
                NetFee = isNaN(NetFee) ? 0 : NetFee;
                //let Expense = parseFloat($('#expense').val());
                let Expense = vm.expense;
                Expense = isNaN(Expense) ? 0 : Expense;
                let RevenueAMT = NetFee + Expense;
                let Tax = Math.round(RevenueAMT * 0.1);
                if (document.getElementById("nonVAT").checked) {
                    Tax = 0;
                }
                let TotalAMT = RevenueAMT + Tax;
                //$('#tax').val(Tax);
                //$('#subtotal').val(RevenueAMT);
                //$('#total').val(TotalAMT);
                vm.vat = Tax;
                vm.subtotal = RevenueAMT;
                vm.total = TotalAMT;
            }

            vm.save = function () {
                console.log(vm.invoiceDate);
                let invoiceDate = moment(vm.invoiceDate, 'DD-MM-YYYY').format();
                let postData = {
                    invoiceCode: vm.invoiceCode
                    , claimID: vm.claimID
                    , customerID: vm.customerID
                    , invoiceDate: invoiceDate
                    , fee: vm.fee
                    , expense: vm.expense
                    , remark: vm.remark
                    , nonVAT: vm.nonVAT
                }
                console.log('create', postData);
                vm.saving = true;
                invoiceService.create(postData).then(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close();
                }).finally(function () {
                    vm.saving = false;
                });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            function init() {
                let claimID = $scope.$parent.vm.claimID;
                vm.claimID = claimID;
                vm.customerID = $scope.$parent.vm.customerID;
                vm.customerName = $scope.$parent.vm.customerName;
                console.log('claimID');
            }

            init();
        }
    ]);
})();