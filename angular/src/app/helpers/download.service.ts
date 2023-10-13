import { Injectable } from '@angular/core';
import { FileBlobDto } from '@proxy/dtos';

@Injectable({
  providedIn: 'root'
})
export class DownloadService {

  constructor() { }

  download(data: FileBlobDto) {
    const source = `data:${data.contentType};base64,${data.content}`;
    const link = document.createElement(`a`);
    link.href = source;
    link.download = data.fileName;
    link.click();
  }
}
