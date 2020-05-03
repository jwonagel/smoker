import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-sensor-item',
  templateUrl: './sensor-item.component.html',
  styleUrls: ['./sensor-item.component.scss']
})
export class SensorItemComponent implements OnInit {

  @Input()
  value?: number;
  @Input()
  icon: string;

  @Input()
  sensorName: string;

  constructor() { }

  ngOnInit(): void {
    if (this.icon === undefined){
      this.icon = 'thermometer-half';
    }
  }

}
