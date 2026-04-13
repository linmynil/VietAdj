(function () {
    appModule.controller('tenant.views.OldList.createOrUpdateClaim', [
        '$scope',
        '$uibModal',
        '$window',
        '$location',
        '$stateParams',
        'abp.services.app.claim',
        'abp.services.app.typeOfLoss',
        'abp.services.app.cause',
        'abp.services.app.claimType',
        'abp.services.app.customer',
        'abp.services.app.employee',
        'abp.services.app.claimFolderService',
        function (
            $scope,
            $uibModal,
            $window,
            $location,
            $stateParams,
            claimService,
            typeOfLossService,
            causeService,
            claimTypeService,
            customerService,
            employeeService,
            claimFolderService
        ) {
            var vm = this;
            vm.listAE = [];
            vm.item = {};
            vm.ACC = true;
            vm.IC = true;
            vm.OC = true;
            vm.amID = null;
            vm.grant_assign = false;
            vm.grant_update_status = false;

            vm.claim = {};
            console.log('$stateParams', $stateParams);
            var claimId = $stateParams.claimId;
            if (!claimId) {
                claimId = $location.search().id;
            }

            vm.item.claimId = claimId;

            $scope.$watch('$viewContentLoaded', function () {
                console.log('>>>>>viewContentLoaded');                
                $('#accountExecs').multiselect({
                    afterMoveToRight: function ($left, $right, $options) {
                        vm.listAE = [];
                        vm.amID = vm.item.accountManagerID;
                        console.log('pitem>>>>');
                        $($right).find('option').each(function () {
                            var pitem = { id: this.value.replace('number:', ''), name: this.text };
                            if (pitem.id == vm.item.accountManagerID) { vm.amID = pitem.id }
                            vm.listAE.push(pitem);
                            console.log('pitem', pitem);
                        });
                        console.log('vm.listAE>>>>', vm.listAE);                        
                        let print = $("[name='aeato']");
                        console.log('DATA :', print);
                        if (vm.listAE.length > 0) {
                            $("#AEAValidate").removeClass("has-error")
                        }                        
                    },
                    afterMoveToLeft: function ($left, $right, $options) {
                        vm.listAE = [];
                        vm.amID = null;
                        console.log('pitem>>>>');
                        $($right).find('option').each(function () {
                            let print = $("[name='aeato']");
                            var pitem = { id: this.value.replace('number:', ''), name: this.text };
                            if (pitem.id == vm.item.accountManagerID) {vm.amID = pitem.id}
                            vm.listAE.push(pitem);
                            console.log('pitem', pitem);
                        });
                        console.log('vm.listAE>>>>', vm.listAE);                        
                        if (vm.listAE.length == 0) {
                            $("#AEAValidate").addClass("has-error")
                            $("#ACValidate").addClass("has-error")
                            vm.ACC = true;
                        }                        
                    },
                    rightSelected: '#js_right_Selected_1',
                    leftSelected: '#js_left_Selected_1',
                    search: {
                        left: '<input type="text" name="q" class="form-control" placeholder="Search..." />'
                    }                    
                });

                $('#multiselect').multiselect({
                    afterMoveToRight: function ($left, $right, $options) {
                        let innn = $("[name='insurerto']");
                        console.log('DATA :', innn);                        
                    },
                    afterMoveToLeft: function ($left, $right, $options) {
                    },
                    search: {
                        left: '<input type="text" name="q2" class="form-control" placeholder="Search..." />'
                    }                    
                });                

                if (vm.item.dateOfAssignment) {
                    let doa = moment(vm.item.dateOfAssignment);
                    $('#doa').daterangepicker({
                        singleDatePicker: true,
                        autoUpdateInput: false,
                        startDate: doa.format('DD-MM-YYYY'),
                        locale: {
                            format: 'DD-MM-YYYY'
                        },
                    });
                } else {
                    $('#doa').daterangepicker({
                        singleDatePicker: true,
                        autoUpdateInput: false,
                        locale: {
                            format: 'DD-MM-YYYY'
                        },
                    });
                    $('#doa').on('apply.daterangepicker', function (ev, picker) {
                        $('#doa').val(picker.startDate.format('DD-MM-YYYY'));
                    });
                }
                $('#dol').daterangepicker({
                    singleDatePicker: true,
                    autoUpdateInput: false,
                    locale: {
                        format: 'DD-MM-YYYY'
                    },
                });
                $('#dol').on('apply.daterangepicker', function (ev, picker) {
                    $('#dol').val(picker.startDate.format('DD-MM-YYYY'));
                });
            });


            //selectedAM = function (item) {
            //    console.log('selected item', item);

            //};

            vm.selectedAM = function () {
                if ((vm.amID != null) && (vm.item.accountManagerID == null)) {                    
                    vm.item.accountManagerID = vm.amID;
                    console.log('vm. select 1 am ', vm.item.accountManagerID);
                    $("#ACValidate").removeClass("has-error")
                    vm.ACC = false;
                } else {
                    console.log('vm. select 2 am ', vm.item.accountManagerID);
                    if (vm.item.accountManagerID != null) {
                        vm.amID = vm.item.accountManagerID;
                        $("#ACValidate").removeClass("has-error")
                        vm.ACC = false;
                    } else {
                        $("#ACValidate").addClass("has-error")
                        vm.ACC = true;
                    }
                }                
            };

            vm.save = function (event) {
                if (vm.ACC != true && vm.IC != true && vm.OC != true) {
                    console.log('save', vm.item);
                    event.preventDefault();
                    if (!vm.item) {
                        return;
                    }
                    var listCoOwner = [];
                    $('#multiselect_to').find('option').each(function () {
                        console.log('save coowner', this.text, this.value);
                        var value = this.value.replace('number:', '');
                        listCoOwner.push(value);
                    });

                    var listAE = [];
                    $('#accountExecs_to').find('option').each(function () {
                        console.log('save ae', this.text, this.value);
                        var value = this.value.replace('number:', '');
                        listAE.push(value);
                    });
                    var am;
                    $('#accountMan').find('option selected').each(function () {
                        console.log('save AM', this);
                        //am = this.value.replace('number:', '');
                        var value = this.value.replace('number:', '');
                    });
                    //vm.item.accountManagerID = am;
                    vm.item.listCoOwner = listCoOwner;
                    vm.item.listAE = listAE;
                    let doa = $('#doa').val() !== '' ? moment($('#doa').val(), "DD-MM-YYYY").format() : undefined;
                    vm.item.dateOfAssignment = doa;
                    let dol = $('#dol').val() !== '' ? moment($('#dol').val(), "DD-MM-YYYY").format() : undefined;
                    //vm.item.dateOfAssignment = doa;
                    vm.item.dateOfLoss = dol;                    
                    console.log('Save item: ', vm.item);
                    claimService.create(vm.item)
                        .then(function (result) {                            
                            //abp.notify.info(app.localize('SavedSuccessfully') + ' ' + result.data);
                            console.log('save result', result);
                            vm.openSuccessClaimModal(result.data, vm.item.claimId ? 'edit' : 'create');
                        })
                        .finally(function () {
                        });
                            
                }
            };

            vm.list = [];
            vm.totalCount = 0;
            vm.page = 1;

            vm.check_assign_permission = function () {
                console.log('Check permission for assign employee');
                claimService.checkPermission_AssignClaim()
                .then(function (result) {
                    vm.grant_assign = result.data;
                    console.log('Assign Permission', vm.grant_assign);
                });
            };

            vm.check_updatestatus_permission = function () {
                console.log('Check permission for update claim status');
                claimService.checkPermission_UpdateStatusClaim()
                .then(function (result) {
                    vm.grant_update_status = result.data;
                    console.log('Update Status Permission', vm.grant_update_status);
                });
            };

            vm.getEmployees = function () {
                if (!vm.item.claimId) {
                    employeeService.getEmployeesForClaim({})
                        .then(function (result) {
                            vm.item.avaiAEs = result.data;                            
                        });
                }
            };
            vm.getTypeOfLoss = function () {
                typeOfLossService.getAll({})
                    .then(function (result) {
                        vm.listTypeOfLoss = result.data;
                    });
            };
            vm.getCauses = function () {
                causeService.getAll({})
                    .then(function (result) {
                        vm.listCauses = result.data;
                    });
            };
            vm.getClaimType = function () {
                claimTypeService.getAll({})
                    .then(function (result) {
                        vm.listClaimTypes = result.data;
                    });
            };
            vm.getCustomers = function () {
                customerService.getByType('C')
                    .then(function (result) {
                        vm.listInsureres = result.data;
                        if (!vm.item.claimId) {
                            vm.item.avaiCoOwners = result.data;
                        }
                    });

                customerService.getByType('B')
                    .then(function (result) {
                        vm.listBrokers = result.data;
                    });
            };

            vm.init = function () {
                if (vm.item.claimId) {
                    App.startPageLoading({ animate: true });
                    claimService.getById(vm.item.claimId)
                        .then(function (result) {
                            vm.item = result.data;
                            console.log('vm.item for edit', vm.item);                            
                            vm.item.claimId = vm.item.id;
                            vm.amID = vm.item.accountManagerID;
                            if (vm.item.accountExecs != 0) $("#AEAValidate").removeClass("has-error")
                            vm.insurerChange();
                            vm.officeChange();                                                        
                            if (vm.item.dateOfAssignment) {
                                let doa = moment(vm.item.dateOfAssignment);
                                $('#doa').daterangepicker({
                                    singleDatePicker: true,
                                    startDate: doa.format('DD-MM-YYYY'),
                                    locale: {
                                        format: 'DD-MM-YYYY'
                                    },
                                });

                            }
                            if (vm.item.dateOfLoss) {
                                let dol = moment(vm.item.dateOfLoss);
                                $('#dol').daterangepicker({
                                    singleDatePicker: true,
                                    startDate: dol.format('DD-MM-YYYY'),
                                    locale: {
                                        format: 'DD-MM-YYYY'
                                    },
                                });

                            }

                            vm.listAE = [];
                            vm.listAccountExecs = [];
                            $.each(vm.item.aEs, function (index, value) {
                                vm.listAE.push({ id: value.employeeId, name: value.name });
                                vm.listAccountExecs.push({ id: value.employeeId, name: value.name });
                            });
                            console.log('List AE: ', vm.listAE);
                            console.log('Acc Man: ', vm.amID);
                            vm.selectedAM();
                            vm.item.coOwnerInsureres = [];
                            $.each(vm.item.coOwners, function (index, value) {
                                vm.item.coOwnerInsureres.push({ id: value.id, name: value.name, brandName: value.brandName });
                            });
                            if (vm.item.currency) {
                                vm.setCurrency(vm.item.currency);
                            }

                        })
                        .finally(function () {
                            App.stopPageLoading();
                        });
                }
                vm.getTypeOfLoss();
                vm.getCauses();
                vm.getClaimType();
                vm.getCustomers();                
                vm.getEmployees();
                vm.check_assign_permission();
                vm.check_updatestatus_permission();
            };

            vm.popupWindow = function (claimId, lang) {
                var url = 'report?claimId=' + claimId + '&lang=' + lang;
                $window.open(url, "popup", "width=850,height=700,left=100,top=100");
            };

            vm.openSuccessClaimModal = function (itemIn, mode) {
                console.log('open edit modal');
                vm.itemIn = itemIn;
                vm.mode = mode;
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/tenant/views/OldList/successClaimModal.cshtml',
                    controller: 'tenant.views.OldList.successClaimModal as vm',
                    backdrop: 'static',
                    resolve: {
                        item: function () {
                            return vm.itemIn;
                        }
                        , mode: function () {
                            return vm.mode;
                        }
                    }
                });

                modalInstance.result.then(function () {
                    console.log('modalinstance');
                });
            };

            vm.init();

            vm.setCurrency = function (item) {
                console.log('setCurrency', item);
                $('#CurrencyButton').html(item + ' <span class="caret" />');
                console.log($('#CurrencyButton'));
                if (item === 'USD') {
                    console.log('set usd length');
                    $('#est').attr('maxlength', 14);
                }
                else {
                    console.log('set else length');
                    $('#est').attr('maxlength', 18);
                }
                vm.item.currency = item;
            };

            vm.selectCause = function () {
                console.log('vm.selectCause', vm.item.causeID);
                if (vm.item.causeID === 18) {
                    $('#otherCause').removeAttr("disabled");
                } else {
                    $('#otherCause').prop("disabled", true);
                }
            };


            vm.officeChange = function () {
                if (vm.item.officeID != 0) {
                    $("#OfficeValidate").removeClass("has-error")
                    vm.OC = false;
                } else {
                    $("#OfficeValidate").addClass("has-error")
                    vm.OC = true;
                }
            };
            vm.insurerChange = function () {
                console.log('New Insurer: ', vm.item.insurerID);
                if (vm.item.insurerID != null) {
                    $("#InsurerValidate").removeClass("has-error")
                    vm.IC = false;
                } else {
                    $("#InsurerValidate").addClass("has-error")
                    vm.IC = true;
                }
            };

            //END
        }
    ]);
})();