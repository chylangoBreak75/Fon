import { Component, inject, signal, TemplateRef, WritableSignal } from '@angular/core';
import { AppService } from '../services/app.services';
import { ModalDismissReasons, NgbDatepickerModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Select, Store } from '@ngxs/store';   //   select subscriptor a los mensajes (no lo ocupo)
import { KafkaState } from '../state/kafka.state';
import { Observable } from 'rxjs';
import { ConnectWebSocket } from '@ngxs/websocket-plugin';


@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})

export class ListComponent {
  private modalService = inject(NgbModal);
  closeResult: WritableSignal<string> = signal('');
  formSenApp: FormGroup;

  error: string = "";
  listApps : any[];
  listAppTypes : any[];
  listAppStatus : any[];

  //@Select(KafkaState.messages)   obsoleto  //   abajo verdaderos messages
  kafkaMessages$: Observable<string | undefined> = this.store.select(KafkaState.messages);    // asigan selector

  constructor(private readonly store: Store,
    private readonly fb: FormBuilder,
    private readonly appService: AppService) {
    this.listApps = [];
    this.listAppTypes = [];
    this.listAppStatus = [];

    {
      this.formSenApp = this.fb.group({
        type: [null, [Validators.required]],
        description: [null, [Validators.required]]
      });
    }
  }

  ngOnInit() {
    this.getList();

    this.appService.typeList().subscribe({
      next: (response) => {
        this.listAppTypes = response;
        //console.log(this.listAppTypes);
      },
      error: (error) => { console.log(error); this.error = error }
    });

    this.appService.statusList().subscribe({
      next: (response) => {
        this.listAppStatus = response;
        console.log(this.listAppStatus);
      },
      error: (error) => { console.log(error); this.error = error }
    });

    this.kafkaMessages$.subscribe({    //  subscripcion y simpre estará escuchando
      next: (message) => {
        console.log(message);
        if(message == "Accepted")
          this.getList();
      },
      error: (error) => console.log(error)
    });

    this.connect();


  }

  connect() {
    this.store.dispatch(new ConnectWebSocket())     //    plug in   incia la conexion
  }

  getList() {
    this.appService.appList().subscribe({
      next: (response) => {
        this.listApps = response;
        //console.log(this.listApps);

      },
      error: (error) => { console.log(error); this.error = error }
    });
  }

  get f() { return this.formSenApp.controls; }

  open(content: TemplateRef<any>) {
		this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then(
			(result) => {
				this.closeResult.set(`Closed with: ${result}`);
        if(this.formSenApp.valid) {
          //console.log(this.f["description"].value);
          const app = {
            type : this.f["type"].value,
            strType: "",
            strStatus: "",
            description: this.f["description"].value
          };
          this.appService.addApp(app).subscribe({
            next:(response) => {
              //console.log(response);
              this.formSenApp.reset();
              this.getList();
            },
            error: (error) => { console.log(error); this.error = error; }
          });
        } else { alert("Invalid form"); }
			},
			(reason) => {
        this.formSenApp.reset();
				// this.closeResult.set(`Dismissed ${this.getDismissReason(reason)}`);
			},
		);
  }



  private getDismissReason(reason: any): string {
		switch (reason) {
			case ModalDismissReasons.ESC:
				return 'by pressing ESC';
			case ModalDismissReasons.BACKDROP_CLICK:
				return 'by clicking on a backdrop';
			default:
				return `with: ${reason}`;
		}
  }

}
