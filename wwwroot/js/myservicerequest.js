
console.log('MyServiceRequest script loaded');

function showDetails(requestId) {
    console.log('showDetails called with ID:', requestId);

    // Find the service box with this request ID
    const serviceBox = document.querySelector(`[data-request-id="${requestId}"]`);
    if (!serviceBox) {
        console.error('Service box not found for ID:', requestId);
        alert('ไม่พบข้อมูลคำขอบริการ');
        return;
    }

    console.log('Service box found:', serviceBox);

    // Get data from attributes
    const providerName = serviceBox.dataset.providerName || '';
    const providerPhone = serviceBox.dataset.providerPhone || '';
    const providerImage = serviceBox.dataset.providerImage || '/image/man.png';
    const appointment = serviceBox.dataset.appointment || '';
    const address = serviceBox.dataset.address || '';
    const contact = serviceBox.dataset.contact || '';
    const jobDetails = serviceBox.dataset.jobDetails || '';
    const remarks = serviceBox.dataset.remarks || '';
    const statusText = serviceBox.dataset.statusText || '';
    const statusColor = serviceBox.dataset.statusColor || '#6b7280';

    const content = `
            <div style="text-align: center; margin-bottom: 20px;">
                <img src="${providerImage}" alt="Provider" 
                     style="width: 100px; height: 100px; border-radius: 50%; object-fit: cover; margin-bottom: 10px;">
                <h6 style="margin: 0; font-size: 18px; font-weight: 600;">${providerName}</h6>
                <p style="margin: 5px 0; color: #6b7280;">${providerPhone}</p>
            </div>
            
            <div style="background: #f9fafb; padding: 15px; border-radius: 8px; margin-bottom: 15px;">
                <div style="margin-bottom: 12px;">
                    <strong style="color: #374151;">วันนัดหมาย:</strong>
                    <p style="margin: 4px 0 0 0; color: #6b7280;">${appointment}</p>
                </div>
                <div style="margin-bottom: 12px;">
                    <strong style="color: #374151;">สถานที่:</strong>
                    <p style="margin: 4px 0 0 0; color: #6b7280;">${address}</p>
                </div>
                <div style="margin-bottom: 12px;">
                    <strong style="color: #374151;">เบอร์ติดต่อ:</strong>
                    <p style="margin: 4px 0 0 0; color: #6b7280;">${contact}</p>
                </div>
                ${jobDetails ? `
                <div style="margin-bottom: 12px;">
                    <strong style="color: #374151;">รายละเอียดงาน:</strong>
                    <p style="margin: 4px 0 0 0; color: #6b7280; white-space: pre-wrap;">${jobDetails}</p>
                </div>
                ` : ''}
                ${remarks ? `
                <div style="margin-bottom: 12px;">
                    <strong style="color: #374151;">หมายเหตุ:</strong>
                    <p style="margin: 4px 0 0 0; color: #6b7280;">${remarks}</p>
                </div>
                ` : ''}
                <div>
                    <strong style="color: #374151;">สถานะ:</strong>
                    <span style="color: ${statusColor}; font-weight: bold; margin-left: 8px;">${statusText}</span>
                </div>
            </div>
        `;

    const modalContent = document.getElementById('modalContent');
    const detailModal = document.getElementById('detailModal');

    if (modalContent && detailModal) {
        modalContent.innerHTML = content;
        detailModal.style.display = 'block';
        console.log('Modal displayed');
    } else {
        console.error('Modal elements not found');
        alert('เกิดข้อผิดพลาดในการแสดง modal');
    }
}

function closeDetailModal() {
    const detailModal = document.getElementById('detailModal');
    if (detailModal) {
        detailModal.style.display = 'none';
    }
}

// Close modal when clicking outside
window.onclick = function (event) {
    const modal = document.getElementById('detailModal');
    if (event.target === modal) {
        closeDetailModal();
    }
}

function cancelRequest(requestId) {
    if (confirm('คุณต้องการยกเลิกคำขอนี้หรือไม่?')) {
        fetch('/ServiceRequest/CancelRequest', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: `requestId=${requestId}`
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert(data.message);
                    location.reload();
                } else {
                    alert('เกิดข้อผิดพลาด: ' + data.message);
                }
            })
            .catch(error => {
                alert('เกิดข้อผิดพลาดในการเชื่อมต่อ');
                console.error('Error:', error);
            });
    }
}

