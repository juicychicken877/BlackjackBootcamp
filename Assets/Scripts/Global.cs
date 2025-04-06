namespace BlackjackNamespace {
    public enum GameAction {
        Hit,
        Stand,
        DoubleDown,
        Split,
        None
    }

    public enum HandState {
        Inactive,
        Active,
        Lost,
        Won
    }
}