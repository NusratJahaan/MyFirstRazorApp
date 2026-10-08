CREATE OR ALTER PROCEDURE sp_GetPendingOffences
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        o.Id,
        o.FileNumber,
        o.CreatedDate,
        ot.Description AS OffenceType,
        c.DefendantName,
        c.ComplaintNumber
    FROM Offences o
    INNER JOIN OffenceLookUps ot ON o.OffenceLookUpId = ot.Id
    INNER JOIN Complaints c ON o.ComplaintId = c.Id
    WHERE o.OffenceStatus = 1  
      AND o.IsApproved = 0
    ORDER BY o.CreatedDate DESC;
END
GO