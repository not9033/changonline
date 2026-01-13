
console.log('MyJobs script loaded');

function updateStatus(requestId, status) {
    if (!confirm(`คุณต้องการ${status === 'accepted' ? 'ตอบรับ' : status === 'rejected' ? 'ปฏิเสธ' : 'ทำเครื่องหมายว่าเสร็จสิ้น'}งานนี้หรือไม่?`)) {
        return;
    }

    fetch('/Provider/UpdateJobStatus', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            requestId: requestId,
            status: status
        })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert(data.message);
                location.reload(); // Reload the page to show updated status
            } else {
                alert('เกิดข้อผิดพลาด: ' + data.message);
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('เกิดข้อผิดพลาดในการอัปเดตสถานะ');
        });
}

function showDetails(requestId) {
    const serviceBox = document.querySelector(`[data-request-id="${requestId}"]`);
    if (!serviceBox) {
        console.error('Service box not found for ID:', requestId);
        alert('ไม่พบข้อมูลงาน');
        return;
    }

    const requesterName = serviceBox.dataset.requesterName || '';
    const requesterPhone = serviceBox.dataset.requesterPhone || '';
    const requesterImage = serviceBox.dataset.requesterImage || '/image/man.png';
    const appointment = serviceBox.dataset.appointment || '';
    const address = serviceBox.dataset.address || '';
    const contact = serviceBox.dataset.contact || '';
    const jobDetails = serviceBox.dataset.jobDetails || '';
    const remarks = serviceBox.dataset.remarks || '';
    const statusText = serviceBox.dataset.statusText || '';
    const statusColor = serviceBox.dataset.statusColor || '#6b7280';

    const content = `
        <div style="text-align: center; margin-bottom: 20px;">
            <img src="${requesterImage}" alt="Requester" 
                 style="width: 100px; height: 100px; border-radius: 50%; object-fit: cover; margin-bottom: 10px;">
            <h6 style="margin: 0; font-size: 18px; font-weight: 600;">${requesterName}</h6>
            <p style="margin: 5px 0; color: #6b7280;">${requesterPhone}</p>
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

    document.getElementById('modalContent').innerHTML = content;
    document.getElementById('detailModal').style.display = 'block';
}

function closeDetailModal() {
    document.getElementById('detailModal').style.display = 'none';
}

window.addEventListener('click', function (event) {
    const modal = document.getElementById('detailModal');
    if (event.target === modal) {
        closeDetailModal();
    }
});
