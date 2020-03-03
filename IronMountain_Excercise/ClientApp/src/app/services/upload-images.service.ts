import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { saveAs } from 'file-saver';

@Injectable({
  providedIn: 'root'
})
export class UploadImagesService {
  apiUrl = '/api/ProcessImage';
  uploadImagesUrl = this.apiUrl + '/images';
  uploadZipUrl = this.apiUrl + '/zip';

  constructor(private _http: HttpClient) { }

  uploadImages(images: FormData) {
    return this._http.post(this.uploadImagesUrl, images);
  }

  uploadZip(zip: FormData): Observable<any> {
    return this._http.post(this.uploadZipUrl, zip);
  }
}
