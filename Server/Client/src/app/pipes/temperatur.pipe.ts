import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'temperatur'
})
export class TemperaturPipe implements PipeTransform {

  transform(value?: number, ...args: unknown[]): unknown {
    if (value === null || value === 0){
      return 'Nicht angeschlossen';
    }
    return (Math.round(value * 10) / 10).toFixed(1) + ' C°';
  }

}
