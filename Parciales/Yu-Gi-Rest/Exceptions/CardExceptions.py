class CardException(Exception):
    """Base exception for card-related errors."""
    pass


class CardNotFoundException(CardException):
    """Raised when a requested card cannot be found."""
    pass


class CardAlreadyExistsException(CardException):
    """Raised when attempting to create a card that already exists."""
    pass


class SOAPConnectionException(CardException):
    """Raised when there is an error communicating with the SOAP gateway.

    original_exception: optional, stores the underlying exception instance.
    """
    def __init__(self, message: str = None, original_exception: Exception = None):
        super().__init__(message or "SOAP connection error")
        self.original_exception = original_exception

    def __str__(self):
        base = super().__str__()
        if self.original_exception:
            return f"{base} (caused by: {self.original_exception})"
        return base


__all__ = [
    "CardException",
    "CardNotFoundException",
    "CardAlreadyExistsException",
    "SOAPConnectionException",
]