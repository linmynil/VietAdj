(function () {
    appModule.controller('tenant.views.OldList.updateBorderaux', [
        '$scope', '$uibModal', '$window', '$location', '$stateParams', 'abp.services.app.claim', 'abp.services.app.typeOfLoss', 'abp.services.app.cause', 'abp.services.app.claimType', 'abp.services.app.customer', 'abp.services.app.employee',
        function ($scope, $uibModal, $window, $location, $stateParams, claimService, typeOfLossService, causeService, claimTypeService, customerService, employeeService) {
            var vm = this;
            vm.listAE = [];
            vm.item = {};
            
            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');
            });
            //control dateofreport
         

          
         
            //end control date of survey

            vm.getOpenClaims = function () {
                claimService.getOpenClaimToBorderaux({})
                    .then(function (result) {
                        console.log('open claims', result.data);
                        vm.item.claims = result.data;
                    });
            };

            vm.selectClaimId = function () {
                console.log('select claim', vm.item.claimId);
                if (vm.item.claimId) {
                    claimService.getBorderauxInfo(vm.item.claimId)
                        .then(function (result) {
                            console.log('getBorderauxInfo', result.data);
                            if (result.data.dateOfSurvey == null) {
                                vm.item.dateOfSurvey == null;
                            } else {
                                vm.item.dateOfSurvey = new Date(result.data.dateOfSurvey);
                                vm.item.dateOfSurvey = moment(vm.item.dateOfSurvey).format('DD-MM-YYYY');
                            }
                            if (result.data.dateOfReport == null) {
                                vm.item.dateOfReport == null;
                            }
                            else {
                                vm.item.dateOfReport = new Date(result.data.dateOfReport);
                                vm.item.dateOfReport = moment(vm.item.dateOfReport).format('DD-MM-YYYY');
                            }
                            
                            if (result.data.recentCorrespondence == null) {
                                vm.item.recentCorrespondence == null;
                            } else {
                                vm.item.recentCorrespondence = new Date(result.data.recentCorrespondence);
                                vm.item.recentCorrespondence = moment(vm.item.recentCorrespondence).format('DD-MM-YYYY');
                            } 
                            vm.item.otherStatus = result.data.otherStatus;
                            vm.item.otherFollowUp = result.data.otherFollowUp;
                            vm.item.progressPayment = result.data.progressPayment;
                            vm.item.reserve = result.data.reserve;
                            vm.item.reportID = result.data.reportID;
                            vm.item.claimStatusID = result.data.claimStatusID;
                            vm.selectStatus();
                            vm.item.followUpID = result.data.followUpID;
                            vm.selectFollowUp();
                        });
                    vm.getBordereauxHistories();
                }
            };

            vm.getListReport = function () {
                claimService.getListReport()
                    .then(function (result) {
                        //console.log('getListReport', result.data);
                        vm.item.listReport = result.data;
                    });
            };

            vm.getActiveBordereauxStatus = function () {
                claimService.getActiveBordereauxStatus()
                    .then(function (result) {
                        //console.log('getActiveBordereauxStatus', result.data);
                        vm.item.listStatus = result.data;
                    });
            };

            vm.getFollowUpList = function () {
                claimService.getFollowUpList()
                    .then(function (result) {
                        //console.log('getFollowUpList', result.data);
                        vm.item.listFollowUp = result.data;
                    });
            };

            vm.getBordereauxHistories = function () {
                claimService.getBordereauxHistories(vm.item.claimId)
                    .then(function (result) {
                        console.log('getBordereauxHistories', result.data);
                        vm.item.history = result.data;
                    });
            };

            vm.save = function (event) {
                
                event.preventDefault();
                //if (vm.item.dateOfSurvey != null) {
                vm.item.dateOfSurvey = $('#dateofsurvey').val() !== '' ?
                    moment($('#dateofsurvey').val(), "DD-MM-YYYY").format() : null;
                //}
                //if (vm.item.dateOfReport != null) {
                vm.item.dateOfReport = $('#dateofreport').val() !== '' ?
                    moment($('#dateofreport').val(), "DD-MM-YYYY").format() : null;
                //}
                vm.item.recentCorrespondence = $('#recentCorrespondence').val() !== '' ?
                    moment($('#recentCorrespondence').val(), "DD-MM-YYYY").format() : vm.item.dateOfReport;
                console.log('Save Data: ', vm.item);
                claimService.createBorderaux(vm.item)
                    .then(function (result) {
                        console.log('save', result.data);
                        return result.data;

                    }).then(function (result) {
                        if (result == 'ok') {
                            vm.selectClaimId();
                            abp.notify.info(app.localize('SavedSuccessfully'));
                        } else {
                            abp.notify.warn(result);
                        }
                    });
            }

            vm.getOpenClaims();
            vm.getListReport();
            vm.getActiveBordereauxStatus();
            vm.getFollowUpList();

            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');
                $('#dateofsurvey').daterangepicker({
                    singleDatePicker: true,
                    autoUpdateInput: false,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                });
                $('#dateofsurvey').on('apply.daterangepicker', function (ev, picker) {
                    $('#dateofsurvey').val(picker.startDate.format('DD-MM-YYYY'));
                });
                
                $('#recentCorrespondence').daterangepicker({
                    singleDatePicker: true,
                    autoUpdateInput: false,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                });
                $('#recentCorrespondence').on('apply.daterangepicker', function (ev, picker) {
                    $('#recentCorrespondence').val(picker.startDate.format('DD-MM-YYYY'));
                });     
                
                $('#dateofreport').daterangepicker({
                    singleDatePicker: true,
                    autoUpdateInput: false,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                });
                $('#dateofreport').on('apply.daterangepicker', function (ev, picker) {
                    $('#dateofreport').val(picker.startDate.format('DD-MM-YYYY'));
                });
            });

            //END

            vm.selectStatus = function () {
                console.log('vm.selectStatus', vm.item.claimStatusID);
                if (vm.item.claimStatusID === 'N ') {
                    $('#otherStatus').removeAttr("disabled");
                } else {
                    $('#otherStatus').prop("disabled", true);
                }
            };

            vm.selectFollowUp = function () {
                console.log('vm.selectStatus', vm.item.followUpID);
                if (vm.item.followUpID === 13) {
                    $('#otherFollowUp').removeAttr("disabled");
                } else {
                    $('#otherFollowUp').prop("disabled", true);
                }
            };


        }
    ]);
})();